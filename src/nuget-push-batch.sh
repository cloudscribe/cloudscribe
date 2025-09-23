#!/bin/bash

# Configuration
REPOS=(
  "cloudscribe/cloudscribe.Web.Localization"
  "cloudscribe/cloudscribe.Web.Pagination"
  "cloudscribe/cloudscribe.Web.Navigation"
  "cloudscribe/cloudscribe.Syndication"
  "cloudscribe/cloudscribe"
  "cloudscribe/cloudscribe.Logging"
  "cloudscribe/cloudscribe.SimpleContent"
  "cloudscribe/dynamic-authorization-policy"
  "cloudscribe/cloudscribe.SimpleContactForm"
  "cloudscribe/cloudscribe.UserProperties.Kvp"
  "GreatHouseBarn/cloudscribe.Messaging"
  "GreatHouseBarn/cloudscribe.Commerce"
  "GreatHouseBarn/cloudscribe.TalkAbout"
  "cloudscribe/pwakit"
)

WORKFLOW_FILE="nuget-push-to-production.yml"
DELAY_SECONDS=60
LOG_FILE="nuget-push-execution-$(date +%Y%m%d-%H%M%S).log"
SKIP_FILE="nuget-push-completed.txt"

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Function to check if repo was already processed
is_already_processed() {
  local repo=$1
  if [ -f "$SKIP_FILE" ]; then
    grep -q "^$repo$" "$SKIP_FILE"
    return $?
  fi
  return 1
}

# Function to mark repo as completed
mark_as_completed() {
  local repo=$1
  echo "$repo" >> "$SKIP_FILE"
  echo -e "${GREEN}✓${NC} Marked $repo as completed in skip file" | tee -a "$LOG_FILE"
}

# Function to run workflow and wait for completion
run_workflow() {
  local repo=$1
  local branch=$2

  echo "[$(date)] Starting workflow for $repo on branch $branch" | tee -a "$LOG_FILE"

  # Trigger the workflow
  gh workflow run "$WORKFLOW_FILE" \
    --repo "$repo" \
    --ref "$branch" 2>&1 | tee -a "$LOG_FILE"

  # Wait a moment for the workflow to register
  sleep 5

  # Get the latest run ID
  RUN_ID=$(gh run list \
    --repo "$repo" \
    --workflow "$WORKFLOW_FILE" \
    --branch "$branch" \
    --limit 1 \
    --json databaseId \
    --jq '.[0].databaseId' 2>/dev/null)

  if [ -z "$RUN_ID" ]; then
    echo -e "${RED}✗${NC} Failed to trigger workflow for $repo" | tee -a "$LOG_FILE"
    return 1
  fi

  echo "Workflow triggered with run ID: $RUN_ID" | tee -a "$LOG_FILE"
  echo -n "Waiting for completion"

  # Wait for completion with timeout (30 minutes max)
  TIMEOUT=1800
  ELAPSED=0

  while [ $ELAPSED -lt $TIMEOUT ]; do
    STATUS=$(gh run view "$RUN_ID" \
      --repo "$repo" \
      --json status \
      --jq '.status' 2>/dev/null)

    CONCLUSION=$(gh run view "$RUN_ID" \
      --repo "$repo" \
      --json conclusion \
      --jq '.conclusion' 2>/dev/null)

    if [ "$STATUS" = "completed" ]; then
      echo ""  # New line after dots
      if [ "$CONCLUSION" = "success" ]; then
        echo -e "${GREEN}✓ SUCCESS${NC}" | tee -a "$LOG_FILE"
        mark_as_completed "$repo"
        return 0
      else
        echo -e "${RED}✗ FAILED${NC} (conclusion: $CONCLUSION)" | tee -a "$LOG_FILE"

        # Check if it might be a "version exists" error
        echo "Checking failure reason..." | tee -a "$LOG_FILE"
        gh run view "$RUN_ID" --repo "$repo" --log-failed 2>/dev/null | tail -20 | tee -a "$LOG_FILE"

        return 1
      fi
    fi

    echo -n "."
    sleep 10
    ELAPSED=$((ELAPSED + 10))
  done

  echo -e "\n${RED}✗ TIMEOUT${NC} after 30 minutes" | tee -a "$LOG_FILE"
  return 1
}

# Pre-flight checks
pre_flight_check() {
  echo "=== Pre-flight Checks ===" | tee -a "$LOG_FILE"

  for repo in "${REPOS[@]}"; do
    echo -n "Checking $repo... "

    # Check if already processed
    if is_already_processed "$repo"; then
      echo -e "${YELLOW}SKIP${NC} (already completed)"
      continue
    fi

    # Check if repo exists and accessible
    gh repo view $repo --json name >/dev/null 2>&1
    if [ $? -ne 0 ]; then
      echo -e "${RED}✗${NC} Repository not accessible"
      return 1
    fi

    # Determine branch
    BRANCH="master"
    gh api repos/$repo/branches/master >/dev/null 2>&1
    if [ $? -ne 0 ]; then
      gh api repos/$repo/branches/main >/dev/null 2>&1
      if [ $? -eq 0 ]; then
        BRANCH="main"
      else
        echo -e "${RED}✗${NC} Neither master nor main branch exists"
        return 1
      fi
    fi

    # Check if workflow exists
    gh workflow view "$WORKFLOW_FILE" --repo $repo >/dev/null 2>&1
    if [ $? -ne 0 ]; then
      echo -e "${RED}✗${NC} Workflow $WORKFLOW_FILE not found"
      return 1
    fi

    echo -e "${GREEN}✓${NC} Ready (branch: $BRANCH)"
  done

  echo "" | tee -a "$LOG_FILE"
  return 0
}

# Main execution
main() {
  echo "=== NuGet Push to Production Batch Execution ===" | tee "$LOG_FILE"
  echo "Start time: $(date)" | tee -a "$LOG_FILE"
  echo "Total repositories: ${#REPOS[@]}" | tee -a "$LOG_FILE"

  if [ -f "$SKIP_FILE" ]; then
    SKIP_COUNT=$(wc -l < "$SKIP_FILE")
    echo -e "${YELLOW}Skip file found with $SKIP_COUNT completed repos${NC}" | tee -a "$LOG_FILE"
  fi

  echo "" | tee -a "$LOG_FILE"

  # Run pre-flight checks
  if ! pre_flight_check; then
    echo -e "${RED}Pre-flight checks failed. Aborting.${NC}" | tee -a "$LOG_FILE"
    exit 1
  fi

  SUCCESSFUL=0
  FAILED=0
  SKIPPED=0

  for i in "${!REPOS[@]}"; do
    repo="${REPOS[$i]}"
    echo "[$((i+1))/${#REPOS[@]}] Processing: $repo" | tee -a "$LOG_FILE"

    # Check if already processed
    if is_already_processed "$repo"; then
      echo -e "${YELLOW}↷ Skipping${NC} (already completed)" | tee -a "$LOG_FILE"
      SKIPPED=$((SKIPPED + 1))
      echo "" | tee -a "$LOG_FILE"
      continue
    fi

    # Determine branch
    BRANCH="master"
    gh api "repos/$repo/branches/master" >/dev/null 2>&1
    if [ $? -ne 0 ]; then
      BRANCH="main"
    fi

    # Run workflow
    if run_workflow "$repo" "$BRANCH"; then
      SUCCESSFUL=$((SUCCESSFUL + 1))
    else
      FAILED=$((FAILED + 1))
      echo -e "${RED}Execution failed for $repo. Stopping batch.${NC}" | tee -a "$LOG_FILE"
      break
    fi

    # Delay before next repo (except for last one)
    if [ $i -lt $((${#REPOS[@]} - 1)) ] && [ $FAILED -eq 0 ]; then
      echo "Waiting $DELAY_SECONDS seconds before next repository..." | tee -a "$LOG_FILE"
      sleep $DELAY_SECONDS
    fi

    echo "" | tee -a "$LOG_FILE"
  done

  # Summary
  echo "=== Execution Summary ===" | tee -a "$LOG_FILE"
  echo -e "${GREEN}Successful: $SUCCESSFUL${NC}" | tee -a "$LOG_FILE"
  echo -e "${YELLOW}Skipped: $SKIPPED${NC}" | tee -a "$LOG_FILE"
  echo -e "${RED}Failed: $FAILED${NC}" | tee -a "$LOG_FILE"
  echo "End time: $(date)" | tee -a "$LOG_FILE"
  echo "Log file: $LOG_FILE"
  echo "Skip file: $SKIP_FILE"

  # Exit with error if any failed
  if [ $FAILED -gt 0 ]; then
    exit 1
  fi
}

# Handle command line arguments
case "${1:-}" in
  --clear|--reset)
    echo "Clearing skip file..."
    rm -f "$SKIP_FILE"
    echo "Skip file removed. All repos will be processed."
    ;;
  --status)
    echo "=== Current Status ==="
    if [ -f "$SKIP_FILE" ]; then
      echo "Completed repos:"
      cat "$SKIP_FILE" | while read repo; do
        echo "  ✓ $repo"
      done
    else
      echo "No repos marked as completed yet."
    fi
    echo ""
    echo "Pending repos:"
    for repo in "${REPOS[@]}"; do
      if ! is_already_processed "$repo"; then
        echo "  ○ $repo"
      fi
    done
    ;;
  --test-single)
    # Test with just the first unprocessed repo
    echo "=== Test Mode: Single Repo ==="
    for repo in "${REPOS[@]}"; do
      if ! is_already_processed "$repo"; then
        REPOS=("$repo")
        echo "Testing with: $repo"
        main
        break
      fi
    done
    ;;
  --help)
    echo "Usage: $0 [OPTIONS]"
    echo ""
    echo "Options:"
    echo "  --clear, --reset  Clear the skip file and process all repos"
    echo "  --status          Show which repos are completed/pending"
    echo "  --test-single     Run on first unprocessed repo only"
    echo "  --help            Show this help message"
    echo ""
    echo "Run without options to process all pending repos."
    ;;
  *)
    main
    ;;
esac
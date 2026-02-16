// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Net;
using System.Text.RegularExpressions;

namespace cloudscribe.Core.Web.Components
{
    /// <summary>
    /// Comprehensive validation for user-editable fields to prevent XSS attacks and HTML/JavaScript injection.
    /// 
    /// Security Context:
    /// - Users can edit their DisplayName and other fields via UI
    /// - These fields appear in alert messages which are rendered with @Html.Raw(alert.Message)
    /// - Therefore, we must prevent HTML/JavaScript characters from being stored in any field used in alerts
    /// - This validator can be applied to any user-editable field that appears in system messages or alerts
    /// 
    /// Validation Approach:
    /// - Detects literal and encoded/escaped HTML (entities, URL encoding, unicode escapes)
    /// - Blocks all protocols that could execute code (javascript, vbscript, data, etc.)
    /// - Rejects suspicious patterns and control characters
    /// 
    /// Extensibility:
    /// - Implement IUserInputValidator to override validation rules
    /// - Register custom implementation in DI container
    /// - Useful for plugins or custom security requirements
    /// 
    /// Usage:
    /// - ManageController.ProfileInfo(): Validates user's own DisplayName
    /// - UserAdminController.UserEdit(): Validates admin-edited DisplayName
    /// - RoleAdminController: Can validate role names
    /// - Other controllers: Use for any field rendered with Html.Raw()
    /// 
    /// Related Files:
    /// - Alerts system: cloudscribe.Web.Common/Views/Shared/AlertsPartial.cshtml
    /// - Alert extension: cloudscribe.Web.Common/ControllerExtensions.cs
    /// </summary>
    public class UserInputValidator : IUserInputValidator
    {
        /// <summary>
        /// Checks if a string contains potentially dangerous HTML/JavaScript characters.
        /// Safe to use for any user-editable field that will be displayed in UI.
        /// 
        /// Comprehensive checks for:
        /// - Literal HTML tags and angle brackets
        /// - HTML entity encoding (&lt;, &#60;, &#x3c;, etc.)
        /// - URL encoding (%3C, %3E, etc.)
        /// - Unicode escapes (\u003c, \x3c, etc.)
        /// - JavaScript protocols and event handlers
        /// - Dangerous protocol handlers (data, vbscript, etc.)
        /// - Suspicious patterns and control characters
        /// </summary>
        /// <param name="value">The value to validate</param>
        /// <returns>True if the value is safe (contains no HTML/script tags), false otherwise</returns>
        public bool IsSafeForDisplay(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return true;
            }

            // CHECK 1: Reject literal HTML tags and angle brackets
            // Catches: <script>, <img onerror=>, <svg>, <iframe>, <!--, -->, etc.
            if (value.Contains("<") || value.Contains(">"))
            {
                return false;
            }

            // CHECK 2: Reject HTML entity-encoded angle brackets
            // Catches: &lt;script&gt;, &#60;script&#62;, &#x3C;script&#x3E;, etc.
            // These would be decoded by browser and executed
            if (value.Contains("&lt;") || value.Contains("&gt;") ||
                value.Contains("&#") || value.Contains("&#x") ||
                value.Contains("&#X"))
            {
                return false;
            }

            // CHECK 3: Reject URL-encoded angle brackets
            // Catches: %3Cscript%3E, %3cscript%3e, etc.
            if (value.Contains("%3C") || value.Contains("%3c") ||
                value.Contains("%3E") || value.Contains("%3e"))
            {
                return false;
            }

            // CHECK 4: Reject Unicode escape sequences for angle brackets
            // Catches: \u003c, \u003e, \x3c, \x3e, etc.
            if (Regex.IsMatch(value, @"\\u00[03][ce]|\\x0*[03][ce]", RegexOptions.IgnoreCase))
            {
                return false;
            }

            // CHECK 5: Reject JavaScript protocols and event handlers
            // Catches: javascript:alert(), onerror=, onclick=, onload=, etc.
            if (Regex.IsMatch(value, @"javascript:|on\w+\s*=", RegexOptions.IgnoreCase))
            {
                return false;
            }

            // CHECK 6: Reject other dangerous protocols
            // Catches: vbscript:, data:text/html, data:application/javascript, mhtml:, mocha:, etc.
            if (Regex.IsMatch(value, @"(vbscript|data|mhtml|mocha|wysiwyg|livescript|view-source):", RegexOptions.IgnoreCase))
            {
                return false;
            }

            // CHECK 7: Reject form action handlers pointing to javascript
            // Catches: formaction="javascript:alert()", action="javascript:alert()", etc.
            if (Regex.IsMatch(value, @"(form)?action\s*=\s*['""]?javascript:", RegexOptions.IgnoreCase))
            {
                return false;
            }

            // CHECK 8: Reject CSS expressions and behaviors
            // Catches: expression(), behavior:, -moz-binding:, etc.
            if (Regex.IsMatch(value, @"(expression|behavior|binding)\s*:", RegexOptions.IgnoreCase))
            {
                return false;
            }

            // CHECK 9: Reject null bytes and control characters
            // These can be used in filter bypass techniques
            if (value.Contains("\0") || Regex.IsMatch(value, @"[\x00-\x08\x0B\x0C\x0E-\x1F]"))
            {
                return false;
            }

            // CHECK 10: Reject embedded template expressions
            // Catches: ${alert()}, #{alert()}, etc. (template injection patterns)
            if (Regex.IsMatch(value, @"[$#{]\{.*\}", RegexOptions.IgnoreCase))
            {
                return false;
            }

            // CHECK 11: Additional comprehensive protocol check (uppercase variations)
            // Catches: JAVASCRIPT:, JaVaScRiPt:, VBSCRIPT:, DATA:, ON*=, etc.
            string upperValue = value.ToUpperInvariant();
            if (upperValue.Contains("JAVASCRIPT:") || 
                upperValue.Contains("VBSCRIPT:") ||
                upperValue.Contains("DATA:") ||
                Regex.IsMatch(upperValue, @"ON[A-Z]+\s*="))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets a user-friendly error message for invalid user input.
        /// Used in ModelState.AddModelError() and string localization.
        /// </summary>
        /// <param name="fieldName">The name of the field being validated (e.g., "Display Name", "Role Name")</param>
        /// <returns>Localization key with field name substituted</returns>
        public string GetErrorMessageKey(string fieldName)
        {
            return $"{fieldName} cannot contain HTML, script tags, or executable content.";
        }

        /// <summary>
        /// Sanitizes user input by HTML-encoding it for safe display.
        /// NOTE: This is a SECONDARY safeguard. PRIMARY defense is validation rejection via IsSafeForDisplay().
        /// Use IsSafeForDisplay() for validation FIRST. Only use Sanitize() if absolutely needed as fallback.
        /// 
        /// This encodes the value to prevent HTML/JavaScript execution by converting special characters
        /// to their HTML entity equivalents. For example:
        /// - &lt;script&gt; becomes &amp;lt;script&amp;gt; (displays as &lt;script&gt; in browser)
        /// </summary>
        /// <param name="value">The value to sanitize</param>
        /// <returns>HTML-encoded value that is safe for display</returns>
        public string Sanitize(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            // HTML-encode everything
            // This converts all special characters to their HTML entity equivalents
            // Ensures even if something slips through validation, it won't execute
            return WebUtility.HtmlEncode(value);
        }
    }
}

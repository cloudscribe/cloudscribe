## Release Notes

### version 8.3.0 - July 2025

#### **@cloudscribe/cloudscribe**
- **[#1099](https://github.com/cloudscribe/cloudscribe/issues/1099)**: Summernote Editor Integration - added support for the Summernote editor as a replacement for CKEditor, while retaining the option to use CKEditor if desired.
- **[#1063](https://github.com/cloudscribe/cloudscribe/issues/1063)**: Fixed several issues in the "browse server" modal when invoked from the Summernote toolbar:
  
  - Restored the missing 'Select' button for image selection.
  - Reinstated the 'Crop' tab in the UI.
  - Corrected the modal title.
  - Addressed regressions caused by previous file manager and Summernote integration changes.
- **[#1111](https://github.com/cloudscribe/cloudscribe/issues/1111)**: Fixed newsletter sign-up widget compatibility with invisible reCAPTCHA:
  
  - Resolved an issue where the newsletter sign-up widget would not submit when invisible reCAPTCHA was enabled.
  - Improved JavaScript handling in `EmailListSignUpPartial` to support async validation and proper script loading.
  - Ensured compatibility for both authenticated and unauthenticated users.
- **[#918](https://github.com/cloudscribe/cloudscribe/issues/918)**: IP Address Blocking
  - Added ability to block specific IP addresses via the admin UI.

- **[#1011](https://github.com/cloudscribe/cloudscribe/issues/1011)**: IP Allowlist (Single IPs & Ranges)
  - Added support to restrict site access to only permitted IP addresses.
  - Supports both individual IPs and IP ranges.

- **[#1097](https://github.com/cloudscribe/cloudscribe/issues/1097)**: API Client Secret Expiry Fix (PGSQL)
  - Fixed saving API client secret expiry dates in PostgreSQL when using UTC.
  - Prevented accidental deletion of client and related data due to date handling.

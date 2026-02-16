// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace cloudscribe.Core.Web.Components
{
    /// <summary>
    /// Service for validating user input fields to prevent XSS and HTML/JavaScript injection.
    /// 
    /// Security Context:
    /// - Validates user-editable fields that may appear in HTML output rendered with Html.Raw()
    /// - Prevents stored XSS attacks through user-controlled data
    /// - Can be overridden or extended in derived applications
    /// 
    /// Usage:
    /// - Inject into controllers: public UserAdminController(IUserInputValidator validator)
    /// - Validate input: if (!_validator.IsSafeForDisplay(userInput))
    /// - Get error message: StringLocalizer[_validator.GetErrorMessageKey(fieldName)]
    /// 
    /// Related:
    /// - Alert system: cloudscribe.Web.Common/Views/Shared/AlertsPartial.cshtml (@Html.Raw)
    /// - Display names in alerts: UserAdminController, ManageController
    /// </summary>
    public interface IUserInputValidator
    {
        /// <summary>
        /// Checks if a string contains potentially dangerous HTML/JavaScript characters.
        /// Safe to use for any user-editable field that will be displayed in UI.
        /// </summary>
        /// <param name="value">The value to validate</param>
        /// <returns>True if the value is safe (contains no HTML/script tags), false otherwise</returns>
        bool IsSafeForDisplay(string value);

        /// <summary>
        /// Gets a user-friendly error message for invalid user input.
        /// Used in ModelState.AddModelError() and string localization.
        /// </summary>
        /// <param name="fieldName">The name of the field being validated (e.g., "Display Name", "Role Name")</param>
        /// <returns>Localization key with field name substituted</returns>
        string GetErrorMessageKey(string fieldName);

        /// <summary>
        /// Sanitizes user input by removing dangerous characters.
        /// NOTE: This is a secondary safeguard. Primary defense is validation rejection via IsSafeForDisplay().
        /// Use IsSafeForDisplay() for validation, use Sanitize() only as a fallback.
        /// </summary>
        /// <param name="value">The value to sanitize</param>
        /// <returns>Sanitized value with dangerous characters removed</returns>
        string Sanitize(string value);
    }
}

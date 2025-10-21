using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace cloudscribe.Web.Common.DataAnnotations
{
    /// <summary>
    /// Negative validation for DisplayName: blocks obvious dangerous characters only.
    /// Disallows control characters (C0 + C1) and HTML angle brackets. Allows everything else.
    /// Empty/null values are treated as valid (use [Required] to enforce non-empty).
    /// </summary>
    public class AllowedNameCharsAttribute : ValidationAttribute, IClientModelValidator
    {
        // .NET regex: forbid control chars (C0/C1) and angle brackets
        public const string NetPattern = @"^[^\x00-\x1F\x7F-\x9F<>]+$";
        // JS regex (browser): same as server; jQuery Validate uses JS regex engine
        public const string JsPattern  = "^[^\\x00-\\x1F\\x7F-\\x9F<>]+$";

        private static readonly Regex _regex = new Regex(NetPattern, RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public AllowedNameCharsAttribute()
        {
            ErrorMessage = ErrorMessage ?? "Contains invalid characters";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;
            var s = value as string;
            if (string.IsNullOrEmpty(s)) return ValidationResult.Success;

            if (_regex.IsMatch(s)) return ValidationResult.Success;

            var displayName = validationContext?.DisplayName ?? validationContext?.MemberName ?? "Value";
            return new ValidationResult(string.Format("{0} {1}", displayName, ErrorMessage));
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-regex", ErrorMessageString ?? "Contains invalid characters");
            // Use JS-safe regex matching server behavior
            MergeAttribute(context.Attributes, "data-val-regex-pattern", JsPattern);
        }

        private static bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            if (attributes.ContainsKey(key)) return false;
            attributes.Add(key, value);
            return true;
        }
    }
}

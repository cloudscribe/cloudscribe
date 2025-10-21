using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace cloudscribe.Web.Common.DataAnnotations
{
    /// <summary>
    /// Validates that a string only contains allowed name characters:
    /// Unicode letters, combining marks, digits, space, underscore, hyphen, period, and apostrophe.
    /// Empty/null values are treated as valid (use [Required] to enforce non-empty).
    /// </summary>
    public class AllowedNameCharsAttribute : ValidationAttribute, IClientModelValidator
    {
        public const string AllowedPattern = "^[\\p{L}\\p{M}\\p{N} _\\-.']+$";

        private static readonly Regex _regex = new Regex(AllowedPattern, RegexOptions.Compiled | RegexOptions.CultureInvariant);

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
            MergeAttribute(context.Attributes, "data-val-regex-pattern", AllowedPattern);
        }

        private static bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            if (attributes.ContainsKey(key)) return false;
            attributes.Add(key, value);
            return true;
        }
    }
}


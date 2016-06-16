// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2015-09-17
// Last Modified:		    2016-06-16
// 

// https://github.com/dotnet/corefx/blob/master/src/System.ComponentModel.Annotations/src/System/ComponentModel/DataAnnotations/RequiredAttribute.cs
// https://github.com/aspnet/Mvc/blob/dev/src/Microsoft.AspNetCore.Mvc.DataAnnotations/Internal/RequiredAttributeAdapter.cs

using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using System.Reflection;
using System.Globalization;

namespace cloudscribe.Web.Common.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class RequiredWhenAttribute : ValidationAttribute, IClientModelValidator
    {
        public RequiredWhenAttribute(string dependentProperty, object targetValue)
        {
            _innerAttribute = new RequiredAttribute();
            DependentProperty = dependentProperty;
            TargetValue = targetValue;
        }

        private bool _checkedForLocalizer;
        private IStringLocalizer _stringLocalizer;

        protected RequiredAttribute _innerAttribute;

        public string DependentProperty { get; set; }
        public object TargetValue { get; set; }

        public bool AllowEmptyStrings
        {
            get
            {
                return _innerAttribute.AllowEmptyStrings;
            }
            set
            {
                _innerAttribute.AllowEmptyStrings = value;
            }
        }

        

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // get a reference to the property this validation depends upon         
            var containerType = validationContext.ObjectInstance.GetType();
            var field = containerType.GetProperty(DependentProperty);
            
           
            if (field != null)
            {
                // get the value of the dependent property
                var dependentValue = field.GetValue(validationContext.ObjectInstance, null);
                // trim spaces of dependent value
                if (dependentValue != null && dependentValue is string)
                {
                    dependentValue = (dependentValue as string).Trim();

                    if (!AllowEmptyStrings && (dependentValue as string).Length == 0)
                    {
                        dependentValue = null;
                    }
                }

                // compare the value against the target value
                if ((dependentValue == null && TargetValue == null) ||
                    (dependentValue != null && (TargetValue.Equals("*") || dependentValue.Equals(TargetValue))))
                {
                    // match => means we should try validating this field
                    if (!_innerAttribute.IsValid(value))
                        // validation failed - return an error
                        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName), new[] { validationContext.MemberName });
                }
            }

            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            CheckForLocalizer(context);
            var errorMessage = GetErrorMessage(context.ModelMetadata.GetDisplayName());
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-requiredwhen", errorMessage);
            MergeAttribute(context.Attributes, "data-val-other", "#" + DependentProperty);
            MergeAttribute(context.Attributes, "data-val-otherval", TargetValue.ToString());

        }

        private static bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            if (attributes.ContainsKey(key))
            {
                return false;
            }
            attributes.Add(key, value);
            return true;
        }

        private void CheckForLocalizer(ClientModelValidationContext context)
        {
            if (!_checkedForLocalizer)
            {
                _checkedForLocalizer = true;

                var services = context.ActionContext.HttpContext.RequestServices;
                var options = services.GetRequiredService<IOptions<MvcDataAnnotationsLocalizationOptions>>();
                var factory = services.GetService<IStringLocalizerFactory>();

                var provider = options.Value.DataAnnotationLocalizerProvider;
                if (factory != null && provider != null)
                {
                    _stringLocalizer = provider(
                        context.ModelMetadata.ContainerType ?? context.ModelMetadata.ModelType,
                        factory);
                }
            }
        }

        private string GetErrorMessage(string displayName)
        {
            if (_stringLocalizer != null &&
                !string.IsNullOrEmpty(ErrorMessage) &&
                string.IsNullOrEmpty(ErrorMessageResourceName) &&
                ErrorMessageResourceType == null)
            {
                return _stringLocalizer[ErrorMessage, displayName];
            }

            return FormatErrorMessage(displayName);
        }
    }
}

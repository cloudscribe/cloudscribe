// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2019-05-16
// Last Modified:		    2019-05-17
// 

using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace cloudscribe.Web.Common.DataAnnotations
{
    public class DateMaxValueAttribute : ValidationAttribute, IClientModelValidator
    {
        public DateMaxValueAttribute(string dependentProperty, string errorMessage, bool includeTime = true) : base(errorMessage)
        {
            DependentProperty = dependentProperty;
            _includeTime = includeTime;
        }

        public string DependentProperty { get; set; }
        private bool _includeTime;
        private bool _checkedForLocalizer;
        private IStringLocalizer _stringLocalizer;


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult validationResult = ValidationResult.Success;
            try
            {

                var containerType = validationContext.ObjectInstance.GetType();
                var field = containerType.GetProperty(DependentProperty);
                var extensionValue = field.GetValue(validationContext.ObjectInstance, null);
                var datatype = extensionValue.GetType();

                if (field == null)
                {
                    return new ValidationResult(String.Format("Unknown property: {0}.", DependentProperty));
                }

                if ((field.PropertyType == typeof(DateTime) || (field.PropertyType.IsGenericType && field.PropertyType == typeof(Nullable<DateTime>))))
                {
                    DateTime toValidate = (DateTime)value;
                    DateTime referenceProperty = (DateTime)field.GetValue(validationContext.ObjectInstance, null);

                    if (toValidate > referenceProperty)
                    {
                        validationResult = new ValidationResult(string.Format(ErrorMessageString, referenceProperty.ToString("s")));
                    }
                }
                else
                {
                    validationResult = new ValidationResult("An error occurred while validating the property. OtherProperty is not of type DateTime");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return validationResult;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            CheckForLocalizer(context);
            var errorMessage = GetErrorMessage("MAXVALUE");
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-datemaxval", errorMessage);
            MergeAttribute(context.Attributes, "data-val-datemaxval-culture", CultureInfo.CurrentCulture.Name);
            MergeAttribute(context.Attributes, "data-val-datemaxval-includetime", _includeTime.ToString().ToLowerInvariant());
            MergeAttribute(context.Attributes, "data-val-datemaxval-otherproperty", "#" + DependentProperty);
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

// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2015-09-17
// Last Modified:		    2016-06-10
// 
// from studying the source code we can see that most of the common validation attributes are here in the System.ComponentModel.DataAnnotations namespace
// https://github.com/dotnet/corefx/blob/master/src/System.ComponentModel.Annotations/src/System/ComponentModel/DataAnnotations
// however those do not implement IClientModelValidator so adapters are used to add those from MVC.DataAnnotations and they can be mostly found here:
// https://github.com/aspnet/Mvc/tree/dev/src/Microsoft.AspNetCore.Mvc.DataAnnotations/Internal
// specific examples of interest
// https://github.com/dotnet/corefx/blob/master/src/System.ComponentModel.Annotations/src/System/ComponentModel/DataAnnotations/CompareAttribute.cs
// https://github.com/aspnet/Mvc/blob/dev/src/Microsoft.AspNetCore.Mvc.DataAnnotations/Internal/CompareAttributeAdapter.cs
// https://github.com/dotnet/corefx/blob/master/src/System.ComponentModel.Annotations/src/System/ComponentModel/DataAnnotations/EmailAddressAttribute.cs
// https://github.com/aspnet/Mvc/blob/dev/src/Microsoft.AspNetCore.Mvc.DataAnnotations/Internal/RequiredAttributeAdapter.cs

// other things of interest 
// https://github.com/aspnet/Mvc/tree/dev/src/Microsoft.AspNetCore.Mvc.Abstractions/ModelBinding/Validation
// https://github.com/aspnet/Mvc/tree/dev/src/Microsoft.AspNetCore.Mvc.DataAnnotations/Internal

// there is one good example of an attribute that is fully implemented in MVC and is self contained with both server side and client side
// without any use of an adapter [Remote(...)]
// https://github.com/aspnet/Mvc/blob/dev/src/Microsoft.AspNetCore.Mvc.ViewFeatures/RemoteAttribute.cs

// client side validation is based on jquery
// https://github.com/aspnet/jquery-validation-unobtrusive/blob/master/jquery.validate.unobtrusive.js

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
    public class EnforceTrueAttribute : ValidationAttribute , IClientModelValidator
    {
        public EnforceTrueAttribute(string otherProperty)
        {
            OtherProperty = otherProperty;
        }

        private bool _checkedForLocalizer;
        private IStringLocalizer _stringLocalizer;

        /// <summary>
        /// other property (should be a bool in a hidden field) which determines if this validation should be applied
        /// for example I want to use this attribute to require a user to check the box
        /// agreeing to the registration agreement, but only if the site registration agreement has been specified
        /// ie some sites may not even have an agreement defined
        /// </summary>
        public string OtherProperty { get; private set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var otherPropertyInfo = validationContext.ObjectType.GetRuntimeProperty(OtherProperty);
            if (otherPropertyInfo == null)
            {
                return ValidationResult.Success;     
            }

            object otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);
            if(otherPropertyValue == null) return ValidationResult.Success;
            if(((bool)otherPropertyValue) == false) return ValidationResult.Success;

            if (value == null) return new ValidationResult("value cannot be null");
            if (value.GetType() != typeof(bool)) throw new InvalidOperationException("can only be used on boolean properties.");
            if( (bool)value == true)
            {
                return ValidationResult.Success;
            }
            
            return new ValidationResult(GetErrorMessage(validationContext.DisplayName));
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name);
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
            MergeAttribute(context.Attributes, "data-val-enforcetrue", errorMessage);
            MergeAttribute(context.Attributes, "data-val-other", "#" + OtherProperty);
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

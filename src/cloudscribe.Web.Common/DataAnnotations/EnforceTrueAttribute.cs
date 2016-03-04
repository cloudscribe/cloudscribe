// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2015-09-17
// Last Modified:		    2015-11-18
// 

using Microsoft.Extensions.Localization;
using Microsoft.AspNet.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.Web.Common.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class EnforceTrueAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return false;
            if (value.GetType() != typeof(bool)) throw new InvalidOperationException("can only be used on boolean properties.");
            return (bool)value == true;
        }

        public override string FormatErrorMessage(string name)
        {
            return "The " + name + " field must be checked in order to continue.";
        }

    }

    public class EnforceTrueAttributeAdapter : DataAnnotationsClientModelValidator<EnforceTrueAttribute>
    {
        public EnforceTrueAttributeAdapter(EnforceTrueAttribute attribute, IStringLocalizer stringLocalizer)
            : base(attribute, stringLocalizer)
        {
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules(
           ClientModelValidationContext context)
        {
            if(context == null) { throw new InvalidOperationException("ClientModelValidationContext cannot be null"); }

            var errorMessage = GetErrorMessage(context.ModelMetadata);
            return new[] { new ModelClientValidationEnforceTrueRule(errorMessage) };
        }
    }

    public class ModelClientValidationEnforceTrueRule : ModelClientValidationRule
    {
        private const string EnforceTrueValidationType = "enforcetrue";

        public ModelClientValidationEnforceTrueRule(string errorMessage) :
            base(EnforceTrueValidationType, errorMessage)
        {
        }
    }
}

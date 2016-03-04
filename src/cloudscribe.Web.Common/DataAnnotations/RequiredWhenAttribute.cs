// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2015-09-17
// Last Modified:		    2015-09-17
// 

using Microsoft.AspNet.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace cloudscribe.Web.Common.DataAnnotations
{
    public class RequiredWhenAttribute : ValidationAttribute
    {
        public RequiredWhenAttribute(string dependentProperty, object targetValue)
        {
            _innerAttribute = new RequiredAttribute();
            DependentProperty = dependentProperty;
            TargetValue = targetValue;
        }

        

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

    }
}

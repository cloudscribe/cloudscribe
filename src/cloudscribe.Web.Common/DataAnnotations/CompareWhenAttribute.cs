//// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
//// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//// Author:					Joe Audette
//// Created:				    2015-09-18
//// Last Modified:		    2015-11-18
//// 

//using Microsoft.Extensions.Localization;
//using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Globalization;
//using System.Reflection;

//namespace cloudscribe.Web.Common.DataAnnotations
//{
//    public class CompareWhenAttribute : ValidationAttribute
//    {
//        public CompareWhenAttribute()
//        {

//        }
//        //public CompareWhenAttribute(string whenProperty, object whenValue, string compareProperty)
//        //{
//        //    WhenProperty = whenProperty;
//        //    WhenValue = whenValue;
//        //    CompareProperty = compareProperty;
//        //}

//        public string WhenProperty { get; set; }
//        public object WhenValue { get; set; }

//        public string CompareProperty { get; set; }

//        public string ComparePropertyDisplayName { get; internal set; }

//        public override string FormatErrorMessage(string name)
//        {
//            string ErrorMessageString = "{0} must match {1}.";
//            return String.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, ComparePropertyDisplayName ?? CompareProperty);
//        }

//        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
//        {
//            PropertyInfo whenPropertyInfo = validationContext.ObjectType.GetProperty(WhenProperty);
//            if(whenPropertyInfo == null)
//            {
//                string errorFormat = "Could not find a property named {0}.";
//                return new ValidationResult(String.Format(CultureInfo.CurrentCulture, errorFormat, WhenProperty));
//            }

//            var dependentValue = whenPropertyInfo.GetValue(validationContext.ObjectInstance, null);
//            if(!IsMatch(dependentValue))
//            {
//                // did not meet When condition so no need to compare
//                return ValidationResult.Success;
//            }

//            PropertyInfo comparePropertyInfo = validationContext.ObjectType.GetProperty(CompareProperty);
//            if (comparePropertyInfo == null)
//            {
//                string errorFormat = "Could not find a property named {0}.";
//                return new ValidationResult(String.Format(CultureInfo.CurrentCulture, errorFormat, CompareProperty));
//            }
            

//            object otherPropertyValue = comparePropertyInfo.GetValue(validationContext.ObjectInstance, null);
//            if (!Equals(value, otherPropertyValue))
//            {
//                if (ComparePropertyDisplayName == null)
//                {
//                    DisplayAttribute disp = comparePropertyInfo.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
//                    if (disp != null)
//                    {
//                        ComparePropertyDisplayName = disp.Name;
//                    }
//                    //OtherPropertyDisplayName = ModelMetadataProviders.Current.GetMetadataForProperty(() 
//                    //=> validationContext.ObjectInstance, validationContext.ObjectType, OtherProperty).GetDisplayName();
//                }
//                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
//            }
//            return null;
//        }

//        private bool IsMatch(object dependentValue)
//        {
//            if (dependentValue != null && dependentValue is string)
//            {
//                dependentValue = (dependentValue as string).Trim();
//            }

//            // compare the value against the target value
//            if ((dependentValue == null && WhenValue == null) ||
//                (dependentValue != null && (WhenValue.Equals("*") || dependentValue.Equals(WhenValue))))
//            {
//                // match 
//                return true;
//            }

//            return false;
//        }

//        //public static string FormatPropertyForClientValidation(string property)
//        //{
//        //    if (property == null)
//        //    {
//        //        throw new ArgumentException("Value cannot be null or empty.", "property");
//        //    }
//        //    return "*." + property;
//        //}

//    }

//    //public class CompareWhenAttributeAdapter : DataAnnotationsClientModelValidator<CompareWhenAttribute>
//    //{
//    //    public CompareWhenAttributeAdapter(CompareWhenAttribute attribute, IStringLocalizer stringLocalizer)
//    //        : base(attribute, stringLocalizer)
//    //    {
//    //    }

//    //    public override IEnumerable<ModelClientValidationRule> GetClientValidationRules(
//    //       ClientModelValidationContext context)
//    //    {
//    //        if (context == null) { throw new InvalidOperationException("ClientModelValidationContext cannot be null"); }

//    //        var errorMessage = GetErrorMessage(context.ModelMetadata);
            
//    //        return new[] { new ModelClientValidationCompareWhenRule(
//    //            this.Attribute.CompareProperty,
//    //            this.Attribute.WhenProperty, 
//    //            this.Attribute.WhenValue, 
//    //            errorMessage)  };
//    //    }
//    //}

//    //public class ModelClientValidationCompareWhenRule : ModelClientValidationRule
//    //{
//    //    private const string CompareWhenValidationType = "comparewhen";

//    //    public ModelClientValidationCompareWhenRule(
//    //        string compareProperty,
//    //        string whenProperty,
//    //        object whenValue,
//    //        string errorMessage) :
//    //        base(CompareWhenValidationType, errorMessage)
//    //    {
//    //        this.ValidationParameters.Add("compareproperty", compareProperty);
//    //        this.ValidationParameters.Add("whenproperty", whenProperty);
//    //        this.ValidationParameters.Add("whenvalue", whenValue.ToString());         
//    //    }
//    //}

//}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Resources;
using System.Web.Mvc;

namespace cloudscribe.Configuration.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class StringLengthAppSettingsAttribute : ValidationAttribute, IClientValidatable 
    {
        public StringLengthAppSettingsAttribute()
        { }

        public StringLengthAppSettingsAttribute(
            int minimumLength, 
            int maximumLength,
            string minLengthKey,
            string maxLengthKey)
        {
            MinimumLength = minimumLength;
            MaximumLength = maximumLength;
            MinLengthKey = minLengthKey;
            MaxLengthKey = maxLengthKey;
        }

        public int MaximumLength { get; set; }
        public int MinimumLength { get; set; }
        public string MinLengthKey { get; set; }
        public string MaxLengthKey { get; set; }

        public override bool IsValid(object value)
        {
            if (!(value is int)) { return false; }
            int intValue = Convert.ToInt32(value);

            MinimumLength = AppSettings.GetInt(MinLengthKey, MinimumLength);
            MaximumLength = AppSettings.GetInt(MaxLengthKey, MaximumLength);

            if (intValue < MinimumLength) { return false; }
            if (intValue > MaximumLength) { return false; }


            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            string errorMessageFormat = ErrorMessageString;
            if(string.IsNullOrEmpty(ErrorMessageString))
            {
                if(
                    (ErrorMessageResourceName.Length > 0)
                    &&(ErrorMessageResourceType != null)
                    )
                {
                    ResourceManager resx = new ResourceManager(ErrorMessageResourceType);
                    errorMessageFormat = resx.GetString(ErrorMessageResourceName, CultureInfo.CurrentUICulture);
                    // we don't need to pass in the property name since it should be in the resource string
                    return string.Format(
                        CultureInfo.CurrentCulture,
                        errorMessageFormat, new object[] { MinimumLength, MaximumLength });
                }
                else
                {
                    errorMessageFormat = "{0} must be between {1} and {2}";
                    
                }

            }

            return string.Format(
                CultureInfo.CurrentCulture,
                errorMessageFormat, new object[] { name, MinimumLength, MaximumLength });

            
           
        }

        #region IClientValidatable Members

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            MinimumLength = AppSettings.GetInt(MinLengthKey, MinimumLength);
            MaximumLength = AppSettings.GetInt(MaxLengthKey, MaximumLength);

            string errorMessageFormat = ErrorMessageString;
            if (string.IsNullOrEmpty(ErrorMessageString))
            {
                if (
                    (ErrorMessageResourceName.Length > 0)
                    && (ErrorMessageResourceType != null)
                    )
                {
                    ResourceManager resx = new ResourceManager(ErrorMessageResourceType);
                    errorMessageFormat = resx.GetString(ErrorMessageResourceName, CultureInfo.CurrentUICulture);
                }
                else
                {
                    errorMessageFormat = metadata.DisplayName + " length must be between {0} and {1}";
                }

            }
            //metadata.DisplayName
            var rules = new ModelClientValidationRangeRule(errorMessageFormat, this.MinimumLength, this.MaximumLength);
            yield return rules;
        }

        #endregion
    }
}

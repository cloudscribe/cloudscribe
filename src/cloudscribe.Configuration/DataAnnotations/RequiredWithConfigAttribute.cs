// Author:					Joe Audette
// Created:					2014-10-30
// Last Modified:			2014-10-30
// 

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;


namespace cloudscribe.Configuration.DataAnnotations
{
    /// <summary>
    /// the idea is to have a required attribute that can be enabled or disabled
    /// from appSettings if needed
    /// </summary>
    public class RequiredWithConfigAttribute : RequiredAttribute, IClientValidatable 
    {
        
        private string requiredKey = string.Empty;
        /// <summary>
        /// the key that will be used in appSettings
        /// if not provided works like a normal RequiredAttribute
        /// </summary>
        public string RequiredKey 
        {
            get { return requiredKey; }
            set { requiredKey = value; } 
        }

        private bool reallyRequired = true;
        /// <summary>
        /// you could set this to false
        /// if you want the behavior as the item is not required by default
        /// but it can be made required from config
        /// whereas with the default true, it is required but this can be turned off from config
        /// </summary>
        public bool ReallyRequired
        {
            get { return reallyRequired; }
            set { reallyRequired = value; }
        }

        public override bool IsValid(object value)
        {
            // if no key is specified then use base class
            if(requiredKey.Length == 0)
            {
                return base.IsValid(value);
            }

            reallyRequired = AppSettings.GetBool(requiredKey, reallyRequired);

            // return true if config key says not required
            if (!reallyRequired) { return true; }

            if (value == null) { return false; }

            int length = value.ToString().Length;
            return (length > 0);

        }

        #region IClientValidatable Members

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            reallyRequired = AppSettings.GetBool(requiredKey, reallyRequired);
            
            if (reallyRequired)
            {
                string errorMessage = ErrorMessageString;
                if (string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage = metadata.DisplayName + " is required";

                }    
                var rules = new ModelClientValidationRequiredRule(errorMessage);
                yield return rules;

            }
            else
            {
                
                yield break;
            }

            
            
        }

        #endregion

    }
}

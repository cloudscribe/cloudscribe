
//http://www.codeproject.com/Articles/541244/Conditional-ASP-NET-MVC-Validation-using-Data-Anno

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;

namespace cloudscribe.Configuration.DataAnnotations
{
    // a controller can implement this interface
    public interface IConditionalAttribute
    {
        bool ApplyAttribute(string action, string field, ConditionalAttribute attr, ModelMetadata metadata, ControllerContext context, bool applyAttribute);
    }

    [AttributeUsage(System.AttributeTargets.Property, AllowMultiple = true)]
    public class ConditionalAttribute : Attribute
    {
        public ConditionalAttribute(string actionName, Type attributeType) :
            this(actionName, attributeType, null, null, null, true)
        {
        }

        public ConditionalAttribute(string actionName, Type attributeType, object constructorParam) :
            this(actionName, attributeType, new object[] { constructorParam }, null, null, true)
        {
        }


        public ConditionalAttribute(string actionName, Type attributeType, string key, string value) :
            this(actionName, attributeType, null, key, value, true)
        {
        }

        public ConditionalAttribute(string actionName, Type attributeType, object constructorParam, string key, string value) :
            this(actionName, attributeType, new object[] { constructorParam }, key, value, true)
        {
        }

        public ConditionalAttribute(string actionName, Type attributeType, object constructorParam, bool apply) :
            this(actionName, attributeType, new object[] { constructorParam }, null, null, apply)
        {

        }

        public ConditionalAttribute(string actionName, Type attributeType, object[] constructorParam, string key, string value, bool apply)
        {
            this.Actions = !String.IsNullOrEmpty(actionName) ? new string[] { actionName } : new string[0];
            this.AttributeType = attributeType;
            this.ConstructorParam = constructorParam;
            this.Apply = apply;

            if (key != null)
                Keys = new string[] { key };
            if (value != null)
                Values = new string[] { value };
        }


        public Type AttributeType { get; set; }
        public object[] ConstructorParam { get; set; }
        public bool Apply { get; set; }

        public string[] Actions { get; set; }
        public string[] Keys { get; set; }
        public object[] Values { get; set; }


    }
}

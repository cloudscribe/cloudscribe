
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
    public class ConditionalAttributeModelValidatorProvider : DataAnnotationsModelValidatorProvider
    {
        virtual protected object GetKeyValue(ConditionalAttribute attr, string key)
        {
            object retValue = null;

            if (attr != null)
            {
                if (!String.IsNullOrEmpty(key))
                {
                    if (attr.Keys != null)
                    {
                        int pos = Array.IndexOf(attr.Keys, key);

                        if (pos != -1)
                        {
                            if (attr.Values != null)
                            {
                                if (pos < attr.Values.Length)
                                    retValue = attr.Values.ElementAt(pos);
                            }
                        }
                    }
                }
            }

            return retValue;
        }

        virtual protected object CreateAttribute(Type type, ConditionalAttribute attr)
        {
            object obj = Activator.CreateInstance(type, attr.ConstructorParam);
            var props = type.GetProperties();
            foreach (var prop in props)
            {
                object value = GetKeyValue(attr, prop.Name);

                if (value != null)
                    prop.SetValue(obj, value);
            }

            return obj;

        }

        virtual protected bool ApplyAttribute(
            ConditionalAttribute attr, 
            ModelMetadata metadata, 
            ControllerContext context, 
            IEnumerable<Attribute> attributes)
        {
            var applyAttribute = false;

            var actionName = context.RouteData.GetRequiredString("action");
            var fieldName = metadata.PropertyName;

            if (
                    (
                        (attr.Apply && (attr.Actions.Length == 0 || Array.IndexOf(attr.Actions, actionName) >= 0))
                        ||
                        (!attr.Apply && (attr.Actions.Length > 0 && Array.IndexOf(attr.Actions, actionName) == -1))
                    )
               )
            {
                applyAttribute = true;
            }
            else
                applyAttribute = false;

            IConditionalAttribute cond = context.Controller as IConditionalAttribute;
            if (cond != null)
                applyAttribute = cond.ApplyAttribute(actionName, fieldName, attr, metadata, context, applyAttribute);

            return applyAttribute;
        }

        private object[] GetCustomAttributes(ModelMetadata metadata)
        {
            object[] attrs = null;

            var containerType = metadata.ContainerType;
            if (containerType != null)
            {
                var propInfo = containerType.GetProperties().Where(e => e.Name == metadata.PropertyName).FirstOrDefault();
                if (propInfo != null)
                {
                    attrs = propInfo.GetCustomAttributes(false);
                }
            }

            return attrs;
        }

        protected override IEnumerable<ModelValidator> GetValidators(
            ModelMetadata metadata, 
            ControllerContext context, 
            IEnumerable<Attribute> attributes)
        {
            List<Attribute> newAttributes = new List<Attribute>(attributes);
            var attrsCustom = GetCustomAttributes(metadata);

            if (attrsCustom != null)
            {
                var attrs = attrsCustom.OfType<ConditionalAttribute>().ToList();

                if (attrs != null)
                {
                    foreach (var attr in attrs)
                    {
                        if (ApplyAttribute(attr, metadata, context, attributes))
                        {
                            Attribute objAttr = CreateAttribute(attr.AttributeType, attr) as Attribute;
                            newAttributes.Add(objAttr);

                        }
                    }
                }
            }
            return base.GetValidators(metadata, context, newAttributes);
        }
    }
}

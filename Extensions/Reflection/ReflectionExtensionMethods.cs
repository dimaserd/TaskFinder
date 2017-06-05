using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Extensions.Reflection
{
    public static class ReflectionExtensionMethods
    {
        #region Reflection

        public static Dictionary<string, object> GetPropertiesDictionary(this object attributes)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            if (attributes != null)
            {
                foreach (var prop in attributes.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    result.Add(prop.Name, prop.GetValue(attributes, null));
                }
            }

            return result;
        }

        public static string GetPropertyName<T>(this Expression<Func<T, object>> lambda)
        {
            var member = lambda.Body as MemberExpression;
            var prop = member.Member as PropertyInfo;
            return prop.Name;
        }

        public static PropertyInfo GetPropertyInfo<T>(this Expression<Func<T, object>> lambda)
        {
            var member = lambda.Body as MemberExpression;
            return member.Member as PropertyInfo;
        }


        public static string GetPropertyValue(this object attributes, string propertyName)
        {
            if (attributes == null)
            {
                return null;
            }

            string result = "";

            foreach (var prop in attributes.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (prop.Name == propertyName)
                {
                    result = prop.GetValue(attributes, null).ToString();
                }
            }

            return result;
        }

        public static string RenderAttributesKeyValuePair(this object attributes)
        {
            string result = "";
            if (attributes != null)
            {
                foreach (var prop in attributes.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    result += $" {prop.Name}=\"{prop.GetValue(attributes, null)}\"";
                }
            }

            return result;
        }

        public static string RenderAttributesKeyValuePairExcept(this object attributes, params string[] exceptNames)
        {
            string result = "";
            if (attributes != null)
            {
                foreach (var prop in attributes.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    if (!exceptNames.Any(x => x == prop.Name))
                    {
                        result += $" {prop.Name}='{prop.GetValue(attributes, null)}'";
                    }

                }
            }

            return result;
        }
        #endregion
    }
}

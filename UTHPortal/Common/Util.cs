using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UTHPortal.Common
{
    public static class Util
    {
        /// <summary>
        /// Returns the string representation of this REST Item using the provided model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string ObjectToString(object model, string displayFormat, string[] displayParams)
        {
            if (displayParams != null && displayParams.Length > 0) {
                string [] displayValues = new string [displayParams.Length];
                for (int i = 0; i < displayValues.Length; i++) {
                    object value = GetNestedPropertyValue(model, displayParams [i]);

                    if (value == null) return string.Empty;

                    displayValues [i] = value.ToString();
                }
                return string.Format(displayFormat, displayValues);
            }
            else if (!string.IsNullOrWhiteSpace(displayFormat)) {
                return displayFormat;
            }
            else return string.Empty;
        }

        /// <summary>
        /// Returns the value of the nested property of the provided object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="dottedPropertyName"></param>
        /// <returns></returns>
        private static object GetNestedPropertyValue(object obj, string dottedPropertyName)
        {
            foreach(string propertyName in dottedPropertyName.Split('.')) {
                if (obj == null) {
                    return null;
                }

                var objType = obj.GetType();
                var info = objType.GetRuntimeProperty(propertyName);
                if (info == null) {
                    return null;
                }

                obj = info.GetValue(obj);
            }
            return obj;
        }
    }
}

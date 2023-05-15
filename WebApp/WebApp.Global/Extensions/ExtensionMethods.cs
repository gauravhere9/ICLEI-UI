using Newtonsoft.Json;
using System.Globalization;
using System.Reflection;
using WebApp.Global.Attributes;

namespace WebApp.Global.Extensions
{
    public static class ExtensionMethods
    {
        //To remove extra spaces from the string
        public static string RemoveExtraSpaces(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            return string.Join(" ", value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
        }

        //To remove all the white spaces from the string
        public static string RemoveAllSpaces(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value.Trim();
            }

            return string.Join("", value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
        }

        //To convert the string into title case
        public static string ConvertTitleCase(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            return textInfo.ToTitleCase(value);
        }

        //To convert the object into JSON string
        public static string ToJSON(this object @object) => JsonConvert.SerializeObject(@object, Formatting.None);

        //To reformat the object
        public static void ReformatString<TSelf>(this TSelf obj)
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

            foreach (PropertyInfo property in obj.GetType().GetProperties(flags))
            {
                Type type = property.PropertyType;
                if (type != typeof(TSelf))
                {
                    continue;
                }
                string propertyValue = (string)property.GetValue(obj, null);

                if (propertyValue == null)
                {
                    continue;
                }

                bool modifiedFlag = false;

                if (property.GetCustomAttributes(typeof(NoConsecutiveSpaceAttribute), false).Any())
                {
                    propertyValue = propertyValue.RemoveExtraSpaces();
                    modifiedFlag = true;
                }

                if (property.GetCustomAttributes(typeof(NoSpaceCharAttribute), false).Any())
                {
                    propertyValue = propertyValue.RemoveAllSpaces();
                    modifiedFlag = true;
                }

                if (property.GetCustomAttributes(typeof(TrimAttribute), false).Any())
                {
                    propertyValue = propertyValue.Trim();
                    modifiedFlag = true;
                }

                if (property.GetCustomAttributes(typeof(TitleCaseAttribute), false).Any())
                {
                    propertyValue = propertyValue.ToLower().ConvertTitleCase();
                    modifiedFlag = true;
                }

                if (property.GetCustomAttributes(typeof(UpperCaseAttribute), false).Any())
                {
                    propertyValue = propertyValue.ToUpper();
                    modifiedFlag = true;
                }

                if (modifiedFlag)
                {
                    property.SetValue(obj, propertyValue, null);
                }
            }
        }
    }
}

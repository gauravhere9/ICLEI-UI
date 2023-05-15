using System.ComponentModel;
using System.Reflection;

namespace WebApp.Global.Extensions
{
    public static class EnumExtension
    {
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();

            string name = Enum.GetName(type, value);

            if (name != null)
            {
                FieldInfo field = type.GetField(name);

                if (field != null)
                {
                    var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

                    if (attribute != null)
                    {
                        return attribute.Description;
                    }
                }
            }

            return value.ToString();
        }
    }
}

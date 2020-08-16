using System;
using System.ComponentModel;
using System.Reflection;

namespace BlazingShortcuts.Utilities
{
    public static class Extensions
    {

        public static string GetKeyDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr =
                           Attribute.GetCustomAttribute(field,
                             typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        //if (string.IsNullOrEmpty(attr.Description))
                        //    return attr.ToString();
                        //else
                        return attr.Description;

                    }
                    else
                        return value.ToString();
                }
            }
            return null;
        }
    }
}

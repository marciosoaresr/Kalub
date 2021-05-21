
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace ProfitManager.Negocio.Implementacao
{
    public static class Enumerador
    {
        public static IEnumerable GetValues(Type enumType)
        {
            return ToList(enumType);
        }

        public static IList ToList(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            ArrayList list = new ArrayList();
            Array enumValues = Enum.GetValues(type);

            foreach (Enum value in enumValues)
                list.Add(new KeyValuePair<Enum, string>(value, GetDescription(value)));
            return list;
        }

        public static string GetDescription(Enum value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            if (value.ToString() == "0") return "";

            string description = value.ToString();
            FieldInfo fieldInfo = value.GetType().GetField(description);
            var attributes =
                (EnumDescriptionAttribute[])
                fieldInfo.GetCustomAttributes(typeof(EnumDescriptionAttribute), false);

            if (attributes.Length > 0)
            {
                description = attributes[0].Description;
            }

            return description;
        }

    }

    /// <summary>
    ///     Provides a description for an enumerated type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field,
        AllowMultiple = false)]
    public sealed class EnumDescriptionAttribute : Attribute
    {
        private string description;

        /// <summary>
        ///     Initializes a new instance of the
        ///     <see cref="EnumDescriptionAttribute" /> class.
        /// </summary>
        /// <param name="description">
        ///     The description to store in this attribute.
        /// </param>
        public EnumDescriptionAttribute(string description)
            : base()
        {
            this.description = description;
        }

        /// <summary>
        ///     Gets the description stored in this attribute.
        /// </summary>
        /// <value>The description stored in the attribute.</value>
        public string Description
        {
            get { return description; }
        }
    }

}

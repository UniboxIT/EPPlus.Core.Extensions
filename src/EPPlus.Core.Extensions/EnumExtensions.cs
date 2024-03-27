using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using EPPlus.Core.Extensions.Attributes;

namespace EPPlus.Core.Extensions
{
    public static class EnumExtensions
    {
        public static object ParseWithAttributeField<TAttribute, TField>(this string value, Type type, string field, bool ignoreCase)
            where TAttribute : Attribute
            where TField : class
        {
            if (value == null)
                throw new ArgumentException("value can not be null");
            if (!type.IsEnum)
                throw new ArgumentException("type is not enum");

            return Enum.GetValues(type).Cast<Enum>().FirstOrDefault(v => value.Equals(v.GetAttributeField<TAttribute, TField>(field) as string, ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture))
                ?? Enum.Parse(type, value, ignoreCase);
        }

        internal static TField GetAttributeField<TAttribute, TField>(this Enum @enum, string field)
            where TAttribute : class
            where TField : class
            =>
            @enum?.GetType().GetMember(@enum?.ToString()).FirstOrDefault()?.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute attr
                ? typeof(TAttribute).GetProperty(field).GetValue(attr) as TField
                : null;
    }
}

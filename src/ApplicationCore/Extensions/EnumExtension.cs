using System.ComponentModel.DataAnnotations;
using System;
using System.Linq;
using System.Reflection;

namespace ApplicationCore.Extensions
{
    public static class EnumExtension
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .FirstOrDefault()?
                            .GetCustomAttribute<DisplayAttribute>()?.Name;
        }
    }
}

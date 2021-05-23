using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace CalorieTracker.Helpers
{
    public static class EnumExtensions
    {
        public static string GetDisplayDescription(this Enum enumValue)
        {
            return enumValue.GetType().GetMember(enumValue.ToString())
                .FirstOrDefault()?
                .GetCustomAttribute<DisplayAttribute>()?
                .GetDescription() ?? "unknown";
        }
    }
}
using System.ComponentModel.DataAnnotations;

namespace EventMarketplace.Domain.Enums;

public static class Extension
{
    public static string GetDisplayName(this Enum enumValue)
    {
        if (enumValue == null) throw new ArgumentNullException(nameof(enumValue), "Enum, value cannot be null");

        var type = enumValue.GetType();
        var memberInfo = type.GetMember(enumValue.ToString());
        if (memberInfo.Length > 0)
        {
            var attributes = memberInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);
            if (attributes.Length > 0)
            {
                return ((DisplayAttribute)attributes[0]).Name;
            }
        }

        return enumValue.ToString();
    }
}
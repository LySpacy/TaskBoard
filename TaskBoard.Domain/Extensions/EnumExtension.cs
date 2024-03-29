using System.ComponentModel.DataAnnotations;

namespace TaskBoard.Domain.Extensions
{
    public static class EnumExtension
    {
        public static string GetDisplayName(this System.Enum @enum)
        {
            var enumType = @enum.GetType();
            var memberInfo = enumType.GetMember(@enum.ToString());
            var displayAttribute = memberInfo.FirstOrDefault()
                ?.GetCustomAttributes(typeof(DisplayAttribute), false)
                .FirstOrDefault() as DisplayAttribute;
            return displayAttribute?.Name ?? @enum.ToString();
        }
    }
}

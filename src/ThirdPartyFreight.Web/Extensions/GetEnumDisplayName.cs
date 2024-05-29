using System.ComponentModel;
using System.Reflection;

namespace ThirdPartyFreight.Web.Extensions;

public static class GetEnumDisplayName
{
    public static string GetDisplayName(Enum enumValue)
    {
        FieldInfo? field = enumValue.GetType().GetField(enumValue.ToString());
        var descriptionAttribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field!, typeof(DescriptionAttribute));
        return descriptionAttribute != null ? descriptionAttribute.Description : enumValue.ToString();
    }
}

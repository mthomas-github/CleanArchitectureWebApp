using System.ComponentModel;

namespace ThirdPartyFreight.Infrastructure.DocuSign;

public enum ApiType
{
    /// <summary>
    /// Rooms API
    /// </summary>
    [Description("reg")]
    Rooms,

    /// <summary>
    /// ESign API
    /// </summary>
    [Description("eg")]
    ESignature,

    /// <summary>
    /// Click API
    /// </summary>
    [Description("ceg")]
    Click,

    /// <summary>
    /// Monitor API
    /// </summary>
    [Description("meg")]
    Monitor,

    /// <summary>
    /// Admin API
    /// </summary>
    [Description("aeg")]
    Admin,
}
public static class ApiTypeExtensions
{
    public static string ToKeywordString(this ApiType val)
    {
        var attributes = (DescriptionAttribute[])val
            .GetType()
            .GetField(val.ToString())!
            .GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attributes.Length > 0 ? attributes[0].Description : string.Empty;
    }
}

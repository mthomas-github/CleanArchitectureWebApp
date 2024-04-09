using Telerik.SvgIcons;

namespace ThirdPartyFreight.Web.Layout;

internal sealed class DrawerItem
{
    public string Text { get; init; }
    public ISvgIcon Icon { get; init; }
    public string Url { get; init; }
    public bool Separator { get; init; }
    public IEnumerable<(string, string)> AdditionalData { get; init; }
}

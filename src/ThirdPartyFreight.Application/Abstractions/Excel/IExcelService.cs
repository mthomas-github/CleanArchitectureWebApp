using ThirdPartyFreight.Application.Shared;

namespace ThirdPartyFreight.Application.Abstractions.Excel;

public interface IExcelService
{
    string CreateRoutingGuide(List<RoutingGuideData> data);
}

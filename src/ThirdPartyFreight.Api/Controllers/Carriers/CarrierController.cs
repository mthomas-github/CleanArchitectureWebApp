using Asp.Versioning;
using ThirdPartyFreight.Application.Carriers.AddCarrier;
using ThirdPartyFreight.Application.Carriers.GetCarrier;
using ThirdPartyFreight.Application.Shared;
using ThirdPartyFreight.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ThirdPartyFreight.Api.Controllers.Carriers;
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/carriers")]
public class CarrierController(ISender sender) : ControllerBase
{
    [HttpGet("{id}")]

    public async Task<IActionResult> GetCarrier(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetCarrierQuery(id);

        Result<CarrierResponse> result = await sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    //[HttpPost]
    //public async Task<IActionResult> AddCarrier(AddCarrierRequest request, CancellationToken cancellationToken)
    //{
    //    var command = new AddCarrierCommand(request.AgreementId, request.CarrierName, request.CarrierAccount, request.CarrierType);

    //    Result result = await sender.Send(command, cancellationToken);

    //    return result.IsSuccess ? Ok() : BadRequest(result.Error);
    //}


}


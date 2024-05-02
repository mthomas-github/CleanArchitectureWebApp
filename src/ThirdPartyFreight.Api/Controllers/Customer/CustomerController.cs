using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ThirdPartyFreight.Application.Customer.GetCustomer;
using ThirdPartyFreight.Application.Shared;
using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Api.Controllers.Customer;
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/customer")]

public class CustomerController(ISender sender) : ControllerBase
{
    [HttpGet("{customerNumber}")]
    public async Task<IActionResult> GetCustomer(int customerNumber, CancellationToken cancellationToken)
    {
        var query = new GetCustomerQuery(customerNumber);

        Result<CustomerResponse> result = await sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

}

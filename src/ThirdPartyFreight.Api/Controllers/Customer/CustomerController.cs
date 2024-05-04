using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ThirdPartyFreight.Application.Customer.GetCustomer;
using ThirdPartyFreight.Application.Customer.GetCustomers;
using ThirdPartyFreight.Application.Shared;
using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Api.Controllers.Customer;
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/customers")]

public class CustomerController(ISender sender) : ControllerBase
{
    [HttpGet("{customerNumber}")]
    public async Task<IActionResult> GetCustomer(string customerNumber, CancellationToken cancellationToken)
    {
        var query = new GetCustomerQuery(customerNumber);

        Result<CustomerSiteResponse> result = await sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpGet]
    public async Task<IActionResult> GetCustomers(CancellationToken cancellationToken)
    {
        var query = new GetCustomersQuery();

        Result<IReadOnlyList<CustomerResponse>> result = await sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value.Take(10000)) : NotFound(result.Error);
    }

}

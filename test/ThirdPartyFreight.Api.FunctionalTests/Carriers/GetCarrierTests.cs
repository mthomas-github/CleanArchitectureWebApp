using System.Net;
using System.Net.Http.Json;
using ThirdPartyFreight.Application.Shared;
using FluentAssertions;
using ThirdPartyFreight.Api.FunctionalTests.Infrastructure;

namespace ThirdPartyFreight.Api.FunctionalTests.Carriers;

public class GetCarrierTests(FunctionalTestWebAppFactory factory) : BaseFunctionalTest(factory)
{
    private const string BaseUrl = "api/v1/carriers";


    [Fact]
    public async Task GetCarrier_ShouldReturnOK_WhenCarrierIsRetrieved()
    {
        // Arrange
        var carrierId = new Guid("C74AD015-00C1-426F-84D1-5411F5D94D93");
        
        // Act
        HttpResponseMessage response = await HttpClient.GetAsync($"{BaseUrl}/{carrierId}");
        
        // Assert
       response.StatusCode.Should().Be(HttpStatusCode.OK);
       CarrierResponse? carrier = await response.Content.ReadFromJsonAsync<CarrierResponse>();
       carrier.Should().NotBeNull();
       carrier!.CarrierId.Should().Be(carrierId);
    }

    [Fact]
    public async Task GetCarrier_ShouldReturnNotFound_WhenCarrierIsNotFound()
    {
        // Arrange
        var carrierId = Guid.NewGuid();
        
        // Act
        HttpResponseMessage response = await HttpClient.GetAsync($"{BaseUrl}/{carrierId}");
        
        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}

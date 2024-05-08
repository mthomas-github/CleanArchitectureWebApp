using System.Net;
using System.Net.Http.Json;
using ThirdPartyFreight.Api.Controllers.Carriers;
using ThirdPartyFreight.Domain.Carriers;
using FluentAssertions;
using ThirdPartyFreight.Api.FunctionalTests.Infrastructure;

namespace ThirdPartyFreight.Api.FunctionalTests.Carriers;

public class AddCarrierTests(FunctionalTestWebAppFactory factory) : BaseFunctionalTest(factory)
{
    private const string BaseUrl = "api/v1/carriers";


    [Fact]
    public async Task AddCarrier_ShouldReturnOK_WhenCarrierIsAdded()
    {
      // Act
      HttpResponseMessage response = await HttpClient.PostAsJsonAsync(BaseUrl, CarrierData.AddTestCarrierRequest);

        // Assert
        response.Should().NotBeNull();
        response.EnsureSuccessStatusCode();
    }

    [Theory]
    [InlineData("7e618ec7-df2b-4aa5-9a4c-2e5f0a296da1", "Carrier1", "Account1", CarrierType.Ltl)]
    [InlineData("8f906c0e-55a0-4c6e-a3c1-0eae24ec12bb", "Carrier2", "Account2", CarrierType.Parcel)]
    [InlineData("fb9e91f3-10db-4db7-b381-ee5f6ac739d0", "", "Account3", CarrierType.Ltl)]
    [InlineData("e58b8694-79b6-4e42-973d-5436115d5488", "Carrier4", "", CarrierType.Parcel)]
    [InlineData("67c8b7e0-6b2f-4583-921a-0d3d8d56d057", "", "Account5", (CarrierType)50)]
    [InlineData("00000000-0000-0000-0000-000000000000", "Carrier6", "Account6", CarrierType.Parcel)]
    [InlineData("af92e142-1982-41e3-ae25-bfd7b1f54857", "Carrier7", "", CarrierType.Ltl)]
    [InlineData("92d19367-1a53-41f7-b345-9acfb0d3f0b8", "Carrier8", "", CarrierType.Parcel)]
    [InlineData("f5862b57-d2b5-4c81-9124-eb7f0a34253b", "Carrier9", "Account9", CarrierType.Ltl)]
    [InlineData("a4d7f8c1-6c96-485f-8a69-1abab3f0b858", "Carrier10", "Account10", (CarrierType)10)]
    public async Task AddCarrier_ShouldReturnBadRequest_WhenCarrierRequestIsInvalid(
        string agreementId, 
        string carrierName, 
        string accountNumber, 
        CarrierType carrierType)
    {
        // Arrange
        var invalidRequest = new AddCarrierRequest(Guid.Parse(agreementId), carrierName, accountNumber, carrierType);

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync(BaseUrl, invalidRequest);

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}

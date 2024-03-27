using System.Net;
using System.Net.Http.Json;
using ThirdPartyFreight.Api.Controllers.Agreements;
using ThirdPartyFreight.Api.Controllers.Sites;
using ThirdPartyFreight.Application.Shared;
using ThirdPartyFreight.Domain.Agreements;
using FluentAssertions;
using ThirdPartyFreight.Api.FunctionalTests.Infrastructure;

namespace ThirdPartyFreight.Api.FunctionalTests.Agreements;

public class AddAgreementTests(FunctionalTestWebAppFactory factory) : BaseFunctionalTest(factory)
{
    private const string BaseUrl = "api/v1/agreements";


    [Theory]
    [InlineData(123, "", "Jane Smith", "jane@example", Status.Completed, AgreementType.Add, SiteType.Normal, "Admin")] // Email is invalid
    [InlineData(0, "Alice Johnson", "Bob Brown", "bob@example.com", Status.Creating, (AgreementType)100, (SiteType)9, "Manager")] // CustomerNumber is not greater than 0
    [InlineData(789, "Emily Davis", "", "invalidemail.com", Status.Failed, (AgreementType)10, SiteType.Normal, "Supervisor")] // CustomerName and ContactName are empty, AgreementType is not in enum
    [InlineData(987, "Michael Lee", "Jennifer White", "jennifer@example.com", (Status)10, AgreementType.Creating, SiteType.Normal, "")] // Status is not in enum, CreatedBy is empty
    [InlineData(654, "Samantha Clark", "David Jones", "david@example.com", Status.Failed, (AgreementType)10, SiteType.Normal, "Manager")] // AgreementType is not in enum
    [InlineData(321, "Daniel Martinez", "Lisa Johnson", "lisa@example.com", Status.Failed, AgreementType.Add, (SiteType)10, "")] // SiteType is not in enum, CreatedBy is empty
    [InlineData(-246, "", "Matthew Anderson", "matthew@example.com", Status.Creating, AgreementType.Add, SiteType.Multiple, "Admin")] // CustomerNumber is not greater than 0, CustomerName is empty
    [InlineData(135, "Andrew Brown", "", "sarah@example.com", Status.Creating, AgreementType.Add, SiteType.Email, "Manager")] // ContactName is empty
    [InlineData(579, "Olivia Miller", "James Davis", "", Status.Failed, (AgreementType)10, SiteType.Normal, "Supervisor")] // Email is empty, AgreementType is not in enum
    [InlineData(0, "", "Emma Moore", "emma@example.com", Status.Completed, AgreementType.Update, SiteType.Normal, "Admin")] // CustomerNumber is not greater than 0, CustomerName is empty

    public async Task AddAgreement_ShouldReturnBadRequest_WhenRequestIsInvalid(int customerNumber,
        string customerName,
        string contactName,
        string contactEmail,
        Status status,
        AgreementType agreementType,
        SiteType siteType,
        string createdBy)
    {
        // Arrange
        var request = new AddAgreementRequest(customerNumber, customerName, contactName, contactEmail, status, agreementType, siteType, createdBy);

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync(BaseUrl, request);

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddAgreement_ShouldReturnOk_WhenRequestIsValid()
    {
        // Arrange
        var request = new AddAgreementRequest(123, "Test", "Test User", "test@test.com", Status.Creating,
            AgreementType.Add, SiteType.Normal, "Test User");

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync(BaseUrl, request);

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task AddAgreement_ShouldReturnAgreementDetail_WhenRequestIsValid()
    {
        // Arrange
        AddAgreementRequest expectedValues = AgreementData.AddTestAgreementRequest;

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync(BaseUrl, AgreementData.AddTestAgreementRequest);
        var agreementId = Guid.Parse(response.Headers.Location!.Segments[4]);
        AddSiteRequest addTestSiteRequest = new(agreementId, "1234", "123 No Where St", "Test", "CA", "92222");
        await HttpClient.PostAsJsonAsync("api/v1/sites/", addTestSiteRequest);
        AgreementResponse? agreement = await HttpClient.GetFromJsonAsync<AgreementResponse>($"{BaseUrl}/{agreementId}");

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        agreement.Should().NotBeNull();
        agreement!.CustomerNumber.Should().Be(expectedValues.CustomerNumber);
        agreement.BusinessName.Should().Be(expectedValues.CustomerName);
        agreement.ContactName.Should().Be(expectedValues.ContactName);
        agreement.ContactEmail.Should().Be(expectedValues.ContactEmail);
        agreement.Status.Should().Be(expectedValues.Status);
        agreement.AgreementType.Should().Be(expectedValues.AgreementType);
        agreement.SiteType.Should().Be(expectedValues.SiteType);
        agreement.CreatedBy.Should().Be(expectedValues.CreatedBy);
    }
}

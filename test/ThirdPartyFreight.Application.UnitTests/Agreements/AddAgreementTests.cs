using ThirdPartyFreight.Application.Abstractions.Clock;
using ThirdPartyFreight.Application.Agreements.AddAgreement;
using ThirdPartyFreight.Application.Exceptions;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Agreements;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace ThirdPartyFreight.Application.UnitTests.Agreements;

public class AddAgreementTests
{
    private static readonly DateTime UtcNow = DateTime.UtcNow;

    private static readonly AddAgreementCommand Command = new(
        CustomerNumber: 1234,
        CustomerName: "Customer Name",
        ContactName: "Contact Name",
        ContactEmail: "test@test.com",
        Status: Status.Creating,
        AgreementType: AgreementType.Add,
        SiteType: SiteType.Creating,
        CreatedBy: new CreatedBy("Test User")
    );

    private readonly AddAgreementCommandHandler _handler;

    private readonly IAgreementRepository _agreementRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    public AddAgreementTests()
    {
        _agreementRepositoryMock = Substitute.For<IAgreementRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        IDateTimeProvider dateTimeProviderMock = Substitute.For<IDateTimeProvider>();
        dateTimeProviderMock.UtcNow.Returns(UtcNow);
        
        _handler = new AddAgreementCommandHandler(
                       _agreementRepositoryMock,
                                  _unitOfWorkMock,
                                  dateTimeProviderMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenUnitOfWorkThrows()
    {
        _unitOfWorkMock
            .SaveChangesAsync()
            .Throws(new ConcurrencyException("Concurrency", new Exception()));

        // Act
        Result<Guid> result = await _handler.Handle(Command, default);

        // Assert
        result.Error.Should().Be(AgreementErrors.NotComplete);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenAgreementIsCreated()
    {
        // Act
        Result<Guid> result = await _handler.Handle(Command, default);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_CallRepository_WhenAgreementIsCreated()
    {
        // Act
        Result<Guid> result = await _handler.Handle(Command, default);

        _agreementRepositoryMock.Received(1).Add(Arg.Is<Agreement>(a => a.Id == result.Value));
    }
}

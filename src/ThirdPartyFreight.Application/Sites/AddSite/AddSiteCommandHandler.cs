using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Shared;
using ThirdPartyFreight.Domain.Sites;
using ThirdPartyFreight.Application.Abstractions.Messaging;

namespace ThirdPartyFreight.Application.Sites.AddSite;

internal sealed class AddSiteCommandHandler : ICommandHandler<AddSiteCommand>
{
    private readonly ISiteRepository _siteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddSiteCommandHandler(ISiteRepository siteRepository, IUnitOfWork unitOfWork)
    {
        _siteRepository = siteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AddSiteCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var site = Site.Create(request.AgreementId, new SiteNumber(request.SiteNumber),
                new Address(request.Street, request.City, request.State, request.ZipCode));

            _siteRepository.Add(site);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(SiteErrors.CannotAdd);
        }
    }
}

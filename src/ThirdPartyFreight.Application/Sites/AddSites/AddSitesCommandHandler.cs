using ThirdPartyFreight.Application.Abstractions.Messaging;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Shared;
using ThirdPartyFreight.Domain.Sites;

namespace ThirdPartyFreight.Application.Sites.AddSites;

internal sealed class AddSitesCommandHandler : ICommandHandler<AddSitesCommand>
{
    private readonly ISiteRepository _siteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddSitesCommandHandler(ISiteRepository siteRepository, IUnitOfWork unitOfWork)
    {
        _siteRepository = siteRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result> Handle(AddSitesCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var sites = request.Sites
                .Select(site =>
                    Site.Create(request.AgreementId,
                        new SiteNumber(site.SiteNumber), 
                        new Address(site.Street, site.City, site.State, site.ZipCode))).ToList();

            _siteRepository.Add(sites);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(SiteErrors.CannotAdd);
        }
    }
}

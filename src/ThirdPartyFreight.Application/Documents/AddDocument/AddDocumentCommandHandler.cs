using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Documents;
using ThirdPartyFreight.Application.Abstractions.Messaging;

namespace ThirdPartyFreight.Application.Documents.AddDocument;

internal sealed class AddDocumentCommandHandler : ICommandHandler<AddDocumentCommand>
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddDocumentCommandHandler(IDocumentRepository documentRepository, IUnitOfWork unitOfWork)
    {
        _documentRepository = documentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AddDocumentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var document = Document.Create(request.AgreementId,
                new Details(request.DocumentName, request.DocumentData, request.Type));

            _documentRepository.Add(document);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(DocumentErrors.CannotAdd);
        }
    }
}

using FluentValidation;

namespace ThirdPartyFreight.Application.Documents.AddDocument;

internal sealed class AddDocumentValidator : AbstractValidator<AddDocumentCommand>
{
    public AddDocumentValidator()
    {
        RuleFor(x => x.AgreementId).NotEmpty();
        RuleFor(x => x.DocumentName).NotEmpty();
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.DocumentData).NotEmpty();
    }
}

using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Domain.Documents;

public static class DocumentErrors
{
    public static readonly Error NotFound = new(
               "Document.Found",
                      "The document with the specified identifier was not found");

    public static readonly Error CannotAdd = new(
        "Document.UnableToAdd",
        "The document with specified identifier was not added.");
}

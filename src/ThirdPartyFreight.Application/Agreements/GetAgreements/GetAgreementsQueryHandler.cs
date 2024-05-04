using System.Data;
using ThirdPartyFreight.Domain.Abstractions;
using Dapper;
using ThirdPartyFreight.Application.Abstractions.Data;
using ThirdPartyFreight.Application.Abstractions.Messaging;
using ThirdPartyFreight.Application.Shared;

namespace ThirdPartyFreight.Application.Agreements.GetAgreements;

internal sealed class GetAgreementsQueryHandler
    : IQueryHandler<GetAgreementsQuery, IReadOnlyList<AgreementResponse>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetAgreementsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }


    public async Task<Result<IReadOnlyList<AgreementResponse>>> Handle(GetAgreementsQuery request, CancellationToken cancellationToken)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql = """
                           SELECT
                             *
                           FROM
                           View_TPFAgreements;
                           """;

        var agreements = new List<AgreementResponse>();

        await connection.QueryAsync<AgreementResponse, SiteResponse, CarrierResponse?, DocumentResponse?, NoteResponse?, EnvelopeResponse?, AgreementResponse>(
            sql,
            (agreement, site, carrier, document, note, envelope) =>
            {
                AgreementResponse? existingAgreement = agreements.Find(a => a.Id == agreement.Id);
                if (existingAgreement == null)
                {
                    existingAgreement = agreement;
                    agreements.Add(existingAgreement);
                }

                // Add unique sites
                if (existingAgreement.Sites.TrueForAll(s => s.SiteId != site.SiteId))
                {
                    existingAgreement.Sites.Add(site);
                }

                // Add unique documents
                if (existingAgreement.Documents != null &&
                    document?.DocumentId != null &&
                    existingAgreement.Documents.TrueForAll(d => d.DocumentId != document.DocumentId))
                {
                    existingAgreement.Documents.Add(document);
                }

                // Add unique carriers
                if (existingAgreement.Carriers != null
                    && carrier?.CarrierId != null
                    && existingAgreement.Carriers.TrueForAll(c => c.CarrierId != carrier.CarrierId))
                {
                    existingAgreement.Carriers.Add(carrier);
                }

                // Add unique envelopes
                existingAgreement.Envelope = envelope;

                // Add unique notes
                if (existingAgreement.Notes != null
                    && note?.NoteId != null
                    && existingAgreement.Notes.TrueForAll(n => n.NoteId != note.NoteId))
                {
                    existingAgreement.Notes.Add(note);
                }


                return existingAgreement;
            },
            splitOn: "Id,SiteId,CarrierId,DocumentId,NoteId,EnvelopeId");

        return agreements;

    }
}

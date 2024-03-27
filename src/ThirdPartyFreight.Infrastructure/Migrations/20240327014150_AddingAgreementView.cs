using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThirdPartyFreight.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingAgreementView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE VIEW View_TPFAgreements AS (
                  SELECT
                  	a.Id,
                  	a.ContactInfo_CustomerNumber CustomerNumber,
                  	a.ContactInfo_CompanyName BusinessName,
                  	a.ContactInfo_CustomerName ContactName,
                  	a.ContactInfo_CustomerEmail ContactEmail,
                  	a.[Status],
                  	a.AgreementType,
                  	a.SiteType,
                  	a.CreatedOnUtc,
                  	a.CreatedBy,
                  	a.ModifiedOnUtc,
                  	a.ModifiedBy,
                  	a.Ticket TicketNumber,
                  	s.Id SiteId,
                  	s.SiteNumber,
                  	s.SiteAddress_Street Street,
                  	s.SiteAddress_City City,
                  	s.SiteAddress_State [State],
                  	s.SiteAddress_ZipCode ZipCode,
                  	c.Id CarrierId,
                  	c.CarrierInfo_CarrierName CarrierName,
                  	c.CarrierInfo_CarrierAccount CarrierAccount,
                  	c.CarrierInfo_CarrierType CarrierType,
                  	d.Id DocumentId,
                  	d.DocumentDetails_DocumentName DocumentName,
                  	d.DocumentDetails_DocumentData DocumentData,
                  	d.DocumentDetails_Type DocumentType,
                  	n.Id NoteId,
                  	n.Content NoteContent,
                  	n.NoteType,
                  	n.CreatedOnUtc CreateAt,
                  	e.Id EnvelopeId,
                  	e.EnvelopeId DocuSignId,
                  	e.EnvelopeStatus,
                  	e.AutoRespondReason,
                  	e.VoidReason,
                  	e.SentOnUtc,
                  	e.CompletedOnUtc,
                  	e.CreatedOnUtc CreatedOn,
                  	e.DeliveredOnUtc,
                  	e.ExpiringOnUtc,
                  	e.InitialSentOnUtc,
                  	e.LastModifiedOnUtc,
                  	e.LastStatusChangedOnUtc
                  FROM
                  TPF_Agreements a
                  JOIN TPF_Sites s
                  	ON s.AgreementId = a.Id
                  LEFT JOIN TPF_Envelopes e
                  	ON e.AgreementId = a.Id
                  LEFT JOIN TPF_Carriers c
                  	ON c.AgreementId = a.Id
                  LEFT JOIN TPF_Documents d
                  	ON d.AgreementId = a.Id
                  LEFT JOIN TPF_Notes n
                  	ON n.AgreementId = a.Id
                  );");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW View_TPFAgreements;");
        }
    }
}

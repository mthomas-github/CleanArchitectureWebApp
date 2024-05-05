using DocuSign.eSign.Model;

namespace ThirdPartyFreight.Application.Abstractions.DocuSign;

public interface IDocuSignService
{
    Task<EnvelopeSummary> SendEnvelopeFromTemplate(string signerEmail, string signerName, string templateId, string customerNumber, string companyName, string shipSites);
    Task<EnvelopesInformation> GetEnvelopesInformation(StatusRequest envelopeStatusRequest);
    Task<RecipientsUpdateSummary> UpdateEnvelopeEmailSettings(string envelopeId, string newEmailAddress, string newName, bool resendEnvelope = true);
    Task<EnvelopeUpdateSummary> VoidEnvelope(string envelopeId, string reason, string status = "voided");
    Task<EnvelopeFormData> GetEnvelopeFormData(string envelopeId);
}

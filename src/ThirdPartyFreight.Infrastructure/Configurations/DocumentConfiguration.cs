using ThirdPartyFreight.Domain.Agreements;
using ThirdPartyFreight.Domain.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ThirdPartyFreight.Infrastructure.Configurations;

internal sealed class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.ToTable("TPF_Documents");

        builder.HasKey(document => document.Id);
        
        builder.OwnsOne(document => document.DocumentDetails);

        builder.HasOne<Agreement>()
            .WithMany()
            .HasForeignKey(document => document.AgreementId);
    }
}
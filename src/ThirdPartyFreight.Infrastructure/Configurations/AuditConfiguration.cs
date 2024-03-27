using ThirdPartyFreight.Domain.Agreements;
using ThirdPartyFreight.Domain.Audits;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ThirdPartyFreight.Infrastructure.Configurations;

internal sealed class AuditConfiguration : IEntityTypeConfiguration<Audit>
{
    public void Configure(EntityTypeBuilder<Audit> builder)
    {
        builder.ToTable("TPF_Audits");

        builder.HasKey(audit => audit.Id);

        builder.OwnsOne(audit => audit.AuditInfo);

        builder.HasOne<Agreement>()
            .WithMany()
            .HasForeignKey(audit => audit.AgreementId);
    }
}

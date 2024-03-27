using ThirdPartyFreight.Domain.Agreements;
using ThirdPartyFreight.Domain.Approvals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ThirdPartyFreight.Infrastructure.Configurations;

internal sealed class ApprovalConfiguration : IEntityTypeConfiguration<Approval>
{
    public void Configure(EntityTypeBuilder<Approval> builder)
    {
        builder.ToTable("TPF_Approvals");

        builder.HasKey(approval => approval.Id);

        builder.HasOne<Agreement>()
            .WithMany()
            .HasForeignKey(approval => approval.AgreementId);
    }
}

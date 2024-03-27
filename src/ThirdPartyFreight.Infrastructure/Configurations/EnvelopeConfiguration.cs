using ThirdPartyFreight.Domain.Agreements;
using ThirdPartyFreight.Domain.Envelopes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ThirdPartyFreight.Infrastructure.Configurations;

public sealed class EnvelopeConfiguration : IEntityTypeConfiguration<Envelope>
{
    public void Configure(EntityTypeBuilder<Envelope> builder)
    {
        builder.ToTable("TPF_Envelopes");

        builder.HasKey(envelope => envelope.Id);

        builder.Property(envelope => envelope.VoidReason)
            .HasMaxLength(100)
            .HasConversion(voidReason => voidReason!.Value, value => new VoidReason(value));

        builder.Property(envelope => envelope.AutoRespondReason)
            .HasMaxLength(100)
            .HasConversion(autoRespondReason => autoRespondReason!.Value, value => new AutoRespondReason(value));

        builder.HasOne<Agreement>()
            .WithMany()
            .HasForeignKey(envelope => envelope.AgreementId);
    }
}


using ThirdPartyFreight.Domain.Agreements;
using ThirdPartyFreight.Domain.Carriers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ThirdPartyFreight.Infrastructure.Configurations;

internal sealed class CarrierConfiguration : IEntityTypeConfiguration<Carrier>
{
    public void Configure(EntityTypeBuilder<Carrier> builder)
    {
        builder.ToTable("TPF_Carriers");

        builder.HasKey(carrier => carrier.Id);

        builder.OwnsOne(carrier => carrier.CarrierInfo);

        builder.HasOne<Agreement>()
            .WithMany()
            .HasForeignKey(carrier => carrier.AgreementId);
    }
}
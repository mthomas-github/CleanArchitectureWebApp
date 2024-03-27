using ThirdPartyFreight.Domain.Agreements;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ThirdPartyFreight.Infrastructure.Configurations;

internal sealed class AgreementConfiguration : IEntityTypeConfiguration<Agreement>
{
    public void Configure(EntityTypeBuilder<Agreement> builder)
    {
        builder.ToTable("TPF_Agreements");

        builder.HasKey(agreement => agreement.Id);
        
        builder.OwnsOne(agreement => agreement.ContactInfo);

        builder.Property(agreement => agreement.CreatedBy)
            .HasMaxLength(200)
            .HasConversion(createdBy => createdBy.Value, value => new CreatedBy(value));

        builder.Property(agreement => agreement.ModifiedBy)
            .HasMaxLength(200)
            .HasConversion(modifiedBy => modifiedBy!.Value, value => new ModifiedBy(value));

        builder.Property(agreement => agreement.Ticket)
            .HasMaxLength(50)
            .HasConversion(ticket => ticket!.Value, value => new Ticket(value));
    }
}

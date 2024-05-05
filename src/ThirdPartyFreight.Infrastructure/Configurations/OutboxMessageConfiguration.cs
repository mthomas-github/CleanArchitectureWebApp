using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThirdPartyFreight.Infrastructure.OutBox;

namespace ThirdPartyFreight.Infrastructure.Configurations;

internal sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("TPF_OutboxMessages");

        builder.HasKey(x => x.Id);

        builder
            .Property(outboxMessage => outboxMessage.Content)
            .HasColumnType("nvarchar(max)");
    }
}

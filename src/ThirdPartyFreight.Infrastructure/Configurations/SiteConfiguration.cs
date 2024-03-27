using ThirdPartyFreight.Domain.Agreements;
using ThirdPartyFreight.Domain.Sites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ThirdPartyFreight.Infrastructure.Configurations;

internal sealed class SiteConfiguration : IEntityTypeConfiguration<Site>
{
    public void Configure(EntityTypeBuilder<Site> builder)
    {
        builder.ToTable("TPF_Sites");

        builder.HasKey(site => site.Id);
        
        builder.OwnsOne(site => site.SiteAddress);

        builder.Property(site => site.SiteNumber)
            .HasMaxLength(200)
            .HasConversion(siteNumber => siteNumber.Value, value => new SiteNumber(value));

        builder.HasOne<Agreement>()
            .WithMany()
            .HasForeignKey(site => site.AgreementId);
    }
}
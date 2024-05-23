using ThirdPartyFreight.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ThirdPartyFreight.Infrastructure.Configurations;

internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("TPF_Roles");

        builder.HasKey(role => role.Id);

        builder
            .HasMany(role => role.Permissions)
            .WithMany()
            .UsingEntity<RolePermission>(join => join.ToTable("TPF_RoleUser"));

        builder.HasData(Role.Registered);
    }
}

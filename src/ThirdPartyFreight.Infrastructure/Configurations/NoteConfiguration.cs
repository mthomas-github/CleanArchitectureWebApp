using ThirdPartyFreight.Domain.Agreements;
using ThirdPartyFreight.Domain.Notes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ThirdPartyFreight.Infrastructure.Configurations;

public sealed class NoteConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.ToTable("TPF_Notes");

        builder.HasKey(note => note.Id);

        builder.Property(note => note.Content)
            .HasMaxLength(200)
            .HasConversion(note => note.Value, value => new Content(value));

        builder.HasOne<Agreement>()
            .WithMany()
            .HasForeignKey(note => note.AgreementId);

    }
}

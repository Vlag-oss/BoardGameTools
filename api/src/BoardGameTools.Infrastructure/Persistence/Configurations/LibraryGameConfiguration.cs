using BoardGameTools.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoardGameTools.Infrastructure.Persistence.Configurations
{
    public class LibraryGameConfiguration : IEntityTypeConfiguration<LibraryGame>
    {
        public void Configure(EntityTypeBuilder<LibraryGame> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Source)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.SourceGameId)
                .HasMaxLength(100);

            builder.HasIndex(x => x.OwnerId);

            builder.HasOne(x => x.Owner)
                .WithMany()
                .HasForeignKey(x => x.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

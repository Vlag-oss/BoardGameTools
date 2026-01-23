using BoardGameTools.Domain.Entities;
using BoardGameTools.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BoardGameTools.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            var emailConverter = new ValueConverter<Email, string>(
                    v => v.Value,
                    v => Email.Create(v));

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Email)
                .HasConversion(emailConverter)
                .HasMaxLength(255)
                .IsRequired();

            builder.HasIndex(x => x.Email).IsUnique();

            builder.Property(x => x.PasswordHash).IsRequired();
        }
    }
}

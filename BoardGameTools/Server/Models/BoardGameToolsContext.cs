using BoardGameTools.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace BoardGameTools.Server.Models;

public partial class BoardGameToolsContext : DbContext
{
    public BoardGameToolsContext()
    {
    }

    public BoardGameToolsContext(DbContextOptions<BoardGameToolsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Card> Cards { get; set; }

    public virtual DbSet<Characteristic> Characteristics { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("Name=BoardGameTools");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Card>(entity =>
        {
            entity.ToTable("Card");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Characteristic).HasColumnName("characteristic");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Value).HasColumnName("value");
        });

        modelBuilder.Entity<Characteristic>(entity =>
        {
            entity.ToTable("Characteristic");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

using EFC.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFC;

public class VIATabloidContext : DbContext
{
    public DbSet<Story> Stories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=viatabloid;Username=postgres;Password=postgres");
        }
    }

    // Add parameterless constructor
    public VIATabloidContext() { }

    public VIATabloidContext(DbContextOptions<VIATabloidContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Story>()
            .Property(e => e.Department)
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<Department>(v));
    }
}
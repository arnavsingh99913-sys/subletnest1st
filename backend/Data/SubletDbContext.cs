using Microsoft.EntityFrameworkCore;
using SubletNest.Models;

namespace SubletNest.Data;

public class SubletDbContext : DbContext
{
    public SubletDbContext(DbContextOptions<SubletDbContext> options) : base(options)
    {
    }

    public DbSet<Listing> Listings => Set<Listing>();
    public DbSet<ListingImage> ListingImages => Set<ListingImage>();
    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
    public DbSet<SharedAmenity> SharedAmenities => Set<SharedAmenity>();
    public DbSet<Inquiry> Inquiries => Set<Inquiry>();
    public DbSet<User> Users => Set<User>();
    public DbSet<ListingAudit> ListingAudits => Set<ListingAudit>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasIndex(u => u.Contact);
            entity.HasIndex(u => u.Name);
        });

        modelBuilder.Entity<Listing>()
            .HasMany(l => l.Images)
            .WithOne()
            .HasForeignKey(img => img.ListingId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Listing>()
            .HasMany(l => l.Inventory)
            .WithOne()
            .HasForeignKey(inv => inv.ListingId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Listing>()
            .HasMany(l => l.SharedAmenities)
            .WithOne()
            .HasForeignKey(sa => sa.ListingId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Listing>()
            .HasMany(l => l.Inquiries)
            .WithOne()
            .HasForeignKey(inq => inq.ListingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

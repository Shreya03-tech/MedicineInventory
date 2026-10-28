
using MedicineInventory.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicineInventory.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Medicine> Medicines => Set<Medicine>();

    public DbSet<MedicineBatch> MedicineBatches => Set<MedicineBatch>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure the Medicines table.
        modelBuilder.Entity<Medicine>(entity =>
        {
            entity.ToTable("Medicines");

            entity.HasKey(m => m.Id);

            entity.Property(m => m.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(m => m.Category)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(m => m.Manufacturer)
                .HasMaxLength(100)
                .IsRequired();

            entity.HasIndex(m => m.Name);
        });

        // Configure the MedicineBatches table.
        modelBuilder.Entity<MedicineBatch>(entity =>
        {
            entity.ToTable("MedicineBatches");

            entity.HasKey(b => b.Id);

            entity.Property(b => b.BatchNumber)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(b => b.SupplierName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(b => b.PurchasePrice)
                .HasPrecision(10, 2);

            entity.Property(b => b.SellingPrice)
                .HasPrecision(10, 2);

            entity.HasIndex(b => b.BatchNumber);

            entity.HasOne(b => b.Medicine)
                .WithMany(m => m.Batches)
                .HasForeignKey(b => b.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}

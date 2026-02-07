using Microsoft.EntityFrameworkCore;
using MsiaPropertyTransaction.Domain.Entities;

namespace MsiaPropertyTransaction.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<PropertyTransaction> PropertyTransactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PropertyTransaction>(entity =>
        {
            entity.ToTable("property_transactions");

            entity.Property(e => e.PropertyType)
                .HasColumnName("property_type")
                .IsRequired();

            entity.Property(e => e.District)
                .HasColumnName("district")
                .IsRequired();

            entity.Property(e => e.Mukim)
                .HasColumnName("mukim")
                .IsRequired();

            entity.Property(e => e.SchemeNameArea)
                .HasColumnName("scheme_name_area")
                .IsRequired();

            entity.Property(e => e.RoadName)
                .HasColumnName("road_name");

            entity.Property(e => e.Tenure)
                .HasColumnName("tenure")
                .IsRequired();

            entity.Property(e => e.LandParcelArea)
                .HasColumnName("land_parcel_area")
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.Unit)
                .HasColumnName("unit");

            entity.Property(e => e.MainFloorArea)
                .HasColumnName("main_floor_area")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(e => e.UnitLevel)
                .HasColumnName("unit_level");

            entity.Property(e => e.PropertyTypeStrata)
                .HasColumnName("property_type_strata")
                .IsRequired();

            entity.Property(e => e.Sector)
                .HasColumnName("sector")
                .IsRequired();

            entity.Property(e => e.State)
                .HasColumnName("state")
                .IsRequired();

            entity.Property(e => e.TransactionPrice)
                .HasColumnName("transaction_price")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(e => e.TransactionDate)
                .HasColumnName("transaction_date")
                .IsRequired();
        });
    }
}
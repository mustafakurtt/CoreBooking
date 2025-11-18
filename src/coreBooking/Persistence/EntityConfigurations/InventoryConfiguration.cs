using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory> builder)
    {
        builder.ToTable("Inventories").HasKey(i => i.Id);

        builder.Property(i => i.Id).HasColumnName("Id").IsRequired();
        builder.Property(i => i.RoomTypeId).HasColumnName("RoomTypeId").IsRequired();
        builder.Property(i => i.Date).HasColumnName("Date").HasColumnType("date").IsRequired(); // Sadece tarih, saat deðil
        builder.Property(i => i.Quantity).HasColumnName("Quantity").IsRequired();
        builder.Property(i => i.Price).HasColumnName("Price").HasColumnType("decimal(18,2)").IsRequired(); // Para birimi
        builder.Property(i => i.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(i => i.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(i => i.DeletedDate).HasColumnName("DeletedDate");
        builder.Property(i => i.RowVersion)
            .HasColumnName("RowVersion")
            .IsRowVersion();
        builder.HasQueryFilter(i => !i.DeletedDate.HasValue);

        builder.HasOne(i => i.RoomType)
            .WithMany(rt => rt.Inventories)
            .HasForeignKey(i => i.RoomTypeId);
    }
}
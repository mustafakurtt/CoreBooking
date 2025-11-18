using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.EntityConfigurations;

public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory> builder)
    {
        builder.ToTable("Inventories").HasKey(i => i.Id);

        // Temel Alanlar
        builder.Property(i => i.Id).HasColumnName("Id").IsRequired();
        builder.Property(i => i.RoomTypeId).HasColumnName("RoomTypeId").IsRequired();

        // Tarih (Sadece tarih kısmı)
        builder.Property(i => i.Date).HasColumnName("Date").HasColumnType("date").IsRequired();

        builder.Property(i => i.Quantity).HasColumnName("Quantity").IsRequired();

        // Audit Alanları
        builder.Property(i => i.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(i => i.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(i => i.DeletedDate).HasColumnName("DeletedDate");

        // ❌ SİLİNEN: builder.Property(i => i.Price)... (Hatanın kaynağı buydu)

        // ✅ EKLENEN: Value Object Ayarı (Price: Money)
        builder.OwnsOne(i => i.Price, p =>
        {
            p.Property(m => m.Amount).HasColumnName("Price_Amount").HasColumnType("decimal(18,2)").IsRequired();
            p.Property(m => m.Currency).HasColumnName("Price_Currency").HasMaxLength(EntityLengths.CurrencyCode).IsRequired();
        });

        // 🛡️ CONCURRENCY TOKEN (Çok Önemli)
        builder.Property(i => i.RowVersion)
            .HasColumnName("RowVersion")
            .IsRowVersion();

        // İlişki
        builder.HasOne(i => i.RoomType)
            .WithMany(rt => rt.Inventories)
            .HasForeignKey(i => i.RoomTypeId);

        // Soft Delete
        builder.HasQueryFilter(i => !i.DeletedDate.HasValue);
    }
}
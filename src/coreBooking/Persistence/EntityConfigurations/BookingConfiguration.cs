using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.EntityConfigurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        // 1. Tablo ve Anahtar Ayarlarý
        builder.ToTable("Bookings").HasKey(b => b.Id);

        // 2. Temel Alanlar (nArchitecture standartlarý)
        builder.Property(b => b.Id).HasColumnName("Id").IsRequired();
        builder.Property(b => b.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(b => b.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(b => b.DeletedDate).HasColumnName("DeletedDate");

        // 3. Booking Özel Alanlarý
        builder.Property(b => b.CustomerId).HasColumnName("CustomerId").IsRequired();
        builder.Property(b => b.RoomTypeId).HasColumnName("RoomTypeId").IsRequired();
        builder.Property(b => b.NumberOfGuests).HasColumnName("NumberOfGuests").IsRequired();
        builder.Property(b => b.Status).HasColumnName("Status").IsRequired();

        // 4. Value Object: Period (DateRange)
        // Veritabanýnda CheckInDate ve CheckOutDate sütunlarýna mapliyoruz.
        builder.OwnsOne(b => b.Period, p =>
        {
            p.Property(d => d.Start).HasColumnName("CheckInDate").HasColumnType("date").IsRequired();
            p.Property(d => d.End).HasColumnName("CheckOutDate").HasColumnType("date").IsRequired();
        });

        // 5. Value Object: TotalPrice (Money)
        // Veritabanýnda TotalPrice_Amount ve TotalPrice_Currency sütunlarýna mapliyoruz.
        builder.OwnsOne(b => b.TotalPrice, p =>
        {
            p.Property(m => m.Amount).HasColumnName("TotalPrice_Amount").HasColumnType("decimal(18,2)").IsRequired();
            p.Property(m => m.Currency).HasColumnName("TotalPrice_Currency").HasMaxLength(EntityLengths.CurrencyCode).IsRequired();
        });

        // 6. Ýliþkiler
        builder.HasOne(b => b.RoomType)
            .WithMany(rt => rt.Bookings)
            .HasForeignKey(b => b.RoomTypeId);

        // User ile tek yönlü iliþki (User'ýn içinde Booking listesi tutmuyoruz)
        builder.HasOne(b => b.Customer)
            .WithMany()
            .HasForeignKey(b => b.CustomerId);

        // 7. Soft Delete Filtresi (Silinmiþleri getirme)
        builder.HasQueryFilter(b => !b.DeletedDate.HasValue);
    }
}
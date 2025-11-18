using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class RoomTypeConfiguration : IEntityTypeConfiguration<RoomType>
{
    public void Configure(EntityTypeBuilder<RoomType> builder)
    {
        builder.ToTable("RoomTypes").HasKey(rt => rt.Id);

        builder.Property(rt => rt.Id).HasColumnName("Id").IsRequired();
        builder.Property(rt => rt.HotelId).HasColumnName("HotelId").IsRequired();
        builder.Property(rt => rt.Name).HasColumnName("Name").IsRequired();
        builder.Property(rt => rt.Capacity).HasColumnName("Capacity").IsRequired();
        builder.Property(rt => rt.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(rt => rt.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(rt => rt.DeletedDate).HasColumnName("DeletedDate");

        builder.HasQueryFilter(rt => !rt.DeletedDate.HasValue);

        builder.HasOne(rt => rt.Hotel)
            .WithMany(h => h.RoomTypes)
            .HasForeignKey(rt => rt.HotelId);

        // 2. Inventory ile iliþki (Bir oda tipinin birçok günlük envanter kaydý olur)
        builder.HasMany(rt => rt.Inventories)
            .WithOne(i => i.RoomType)
            .HasForeignKey(i => i.RoomTypeId);

        // 3. Booking ile iliþki (Bir oda tipine birçok rezervasyon yapýlýr)
        builder.HasMany(rt => rt.Bookings)
            .WithOne(b => b.RoomType)
            .HasForeignKey(b => b.RoomTypeId);
    }
}
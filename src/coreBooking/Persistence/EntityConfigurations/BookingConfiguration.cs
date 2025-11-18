using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings").HasKey(b => b.Id);

        builder.Property(b => b.Id).HasColumnName("Id").IsRequired();
        builder.Property(b => b.CustomerId).HasColumnName("CustomerId").IsRequired();
        builder.Property(b => b.RoomTypeId).HasColumnName("RoomTypeId").IsRequired();
        builder.Property(b => b.CheckInDate).HasColumnName("CheckInDate").HasColumnType("date").IsRequired();
        builder.Property(b => b.CheckOutDate).HasColumnName("CheckOutDate").HasColumnType("date").IsRequired();

        builder.Property(b => b.NumberOfGuests).HasColumnName("NumberOfGuests").IsRequired();
        builder.Property(b => b.TotalPrice).HasColumnName("TotalPrice").HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(b => b.Status).HasColumnName("Status").IsRequired();
        builder.Property(b => b.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(b => b.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(b => b.DeletedDate).HasColumnName("DeletedDate");

        builder.HasQueryFilter(b => !b.DeletedDate.HasValue);

        builder.HasOne(b => b.RoomType)
            .WithMany(rt => rt.Bookings)
            .HasForeignKey(b => b.RoomTypeId);

        // 2. Customer (User) ile iliþki
        // User entity'sine gidip "ICollection<Booking>" eklemediðimiz için,
        // "WithMany()" içini boþ býrakýyoruz. Bu "tek yönlü navigasyon" demektir.
        builder.HasOne(b => b.Customer)
            .WithMany()
            .HasForeignKey(b => b.CustomerId);
    }
}
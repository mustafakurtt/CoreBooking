using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.ToTable("Hotels").HasKey(h => h.Id);

        builder.Property(h => h.Id).HasColumnName("Id").IsRequired();
        builder.Property(h => h.Name).HasColumnName("Name").IsRequired();
        builder.Property(h => h.City).HasColumnName("City").IsRequired();
        builder.Property(h => h.Address).HasColumnName("Address").IsRequired();
        builder.Property(h => h.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(h => h.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(h => h.DeletedDate).HasColumnName("DeletedDate");

        builder.HasQueryFilter(h => !h.DeletedDate.HasValue);

        builder.HasMany(h => h.RoomTypes)
            .WithOne(rt => rt.Hotel)
            .HasForeignKey(rt => rt.HotelId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
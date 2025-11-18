using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.EntityConfigurations;

public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.ToTable("Hotels").HasKey(h => h.Id);

        // Temel Alanlar
        builder.Property(h => h.Id).HasColumnName("Id").IsRequired();
        builder.Property(h => h.Name).HasColumnName("Name").HasMaxLength(EntityLengths.Name).IsRequired();
        builder.Property(h => h.City).HasColumnName("City").HasMaxLength(EntityLengths.City).IsRequired();

        builder.Property(h => h.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(h => h.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(h => h.DeletedDate).HasColumnName("DeletedDate");

        // ❌ SİLİNEN: builder.Property(h => h.Address)... (Burası hataya sebep oluyordu)

        // ✅ EKLENEN: Value Object Ayarı (Address)
        builder.OwnsOne(h => h.Address, a =>
        {
            a.Property(p => p.Street).HasColumnName("Address_Street").HasMaxLength(EntityLengths.AddressLine).IsRequired();
            a.Property(p => p.City).HasColumnName("Address_City").HasMaxLength(EntityLengths.City).IsRequired();
            a.Property(p => p.Country).HasColumnName("Address_Country").HasMaxLength(EntityLengths.City).IsRequired();
            a.Property(p => p.ZipCode).HasColumnName("Address_ZipCode").HasMaxLength(EntityLengths.ZipCode).IsRequired();
        });

        // İlişkiler
        builder.HasMany(h => h.RoomTypes)
            .WithOne(rt => rt.Hotel)
            .HasForeignKey(rt => rt.HotelId)
            .OnDelete(DeleteBehavior.Cascade);

        // Soft Delete
        builder.HasQueryFilter(h => !h.DeletedDate.HasValue);
    }
}
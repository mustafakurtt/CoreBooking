using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.EntityConfigurations;

public class GuestConfiguration : IEntityTypeConfiguration<Guest>
{
    public void Configure(EntityTypeBuilder<Guest> builder)
    {
        builder.ToTable("Guests").HasKey(g => g.Id);

        // Temel Alanlar
        builder.Property(g => g.Id).HasColumnName("Id").IsRequired();
        builder.Property(g => g.BookingId).HasColumnName("BookingId").IsRequired();

        // String Uzunluk Kýsýtlamalarý (Shared'dan)
        builder.Property(g => g.FirstName).HasColumnName("FirstName").HasMaxLength(EntityLengths.Name).IsRequired();
        builder.Property(g => g.LastName).HasColumnName("LastName").HasMaxLength(EntityLengths.Name).IsRequired();
        builder.Property(g => g.Nationality).HasColumnName("Nationality").HasMaxLength(EntityLengths.Nationality).IsRequired();

        builder.Property(g => g.DateOfBirth).HasColumnName("DateOfBirth").HasColumnType("date").IsRequired();
        builder.Property(g => g.IsPrimary).HasColumnName("IsPrimary").IsRequired();

        // Audit
        builder.Property(g => g.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(g => g.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(g => g.DeletedDate).HasColumnName("DeletedDate");

        // Ýliþki: Bir Booking'in birden çok Guest'i vardýr.
        builder.HasOne(g => g.Booking)
            .WithMany(b => b.Guests)
            .HasForeignKey(g => g.BookingId)
            .OnDelete(DeleteBehavior.Cascade); // Rezervasyon silinirse misafirler de silinsin.

        builder.HasQueryFilter(g => !g.DeletedDate.HasValue);
    }
}
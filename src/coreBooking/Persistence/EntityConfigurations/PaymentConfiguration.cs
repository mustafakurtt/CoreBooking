using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Persistence.EntityConfigurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments").HasKey(p => p.Id);

        builder.Property(p => p.Id).HasColumnName("Id").IsRequired();
        builder.Property(p => p.BookingId).HasColumnName("BookingId").IsRequired();

        // Ödeme tarihi saatli olmalý (Dakikasý bile önemli)
        builder.Property(p => p.Date).HasColumnName("Date").IsRequired();

        builder.Property(p => p.TransactionId).HasColumnName("TransactionId").HasMaxLength(EntityLengths.TransactionId).IsRequired();
        builder.Property(p => p.Status).HasColumnName("Status").IsRequired();

        // --- VALUE OBJECT AYARI (Money) ---
        // Payment tablosunda Amount_Amount ve Amount_Currency kolonlarý oluþacak
        builder.OwnsOne(p => p.Amount, m =>
        {
            m.Property(x => x.Amount).HasColumnName("Amount_Amount").HasColumnType("decimal(18,2)").IsRequired();
            m.Property(x => x.Currency).HasColumnName("Amount_Currency").HasMaxLength(EntityLengths.CurrencyCode).IsRequired();
        });

        // Audit
        builder.Property(p => p.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(p => p.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(p => p.DeletedDate).HasColumnName("DeletedDate");

        // Ýliþki: Bir Booking'in birden çok ödeme hareketi olabilir (Ön ödeme, kalan ödeme, iade vs.)
        builder.HasOne(p => p.Booking)
            .WithMany(b => b.Payments)
            .HasForeignKey(p => p.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(p => !p.DeletedDate.HasValue);
    }
}
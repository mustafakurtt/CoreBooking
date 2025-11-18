using Domain.ValueObjects;
using NArchitecture.Core.Persistence.Repositories;
using Shared.Enums;

namespace Domain.Entities;

public class Payment : Entity<Guid>
{
    public Guid BookingId { get; set; }
    public Money Amount { get; set; }
    public DateTime Date { get; set; }
    public string TransactionId { get; set; }
    public PaymentStatus Status { get; set; }

    // İlişki
    public virtual Booking Booking { get; set; }
}
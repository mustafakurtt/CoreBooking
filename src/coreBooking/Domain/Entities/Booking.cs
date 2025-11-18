
using Domain.ValueObjects;
using NArchitecture.Core.Persistence.Repositories;
using Shared.Enums;

namespace Domain.Entities;

public class Booking : Entity<Guid>
{
    public Guid CustomerId { get; set; }
    public Guid RoomTypeId { get; set; }

    // REPLACED: CheckInDate & CheckOutDate
    // NEW: Encapsulated date range logic (Start, End, Days calculation)
    public DateRange Period { get; set; }

    public int NumberOfGuests { get; set; }

    // REPLACED: decimal TotalPrice
    // NEW: Structured Money object
    public Money TotalPrice { get; set; }

    public BookingStatus Status { get; set; }

    // Relationships
    public virtual User Customer { get; set; }
    public virtual RoomType RoomType { get; set; }
    public virtual ICollection<Guest> Guests { get; set; }
    public virtual ICollection<Payment> Payments { get; set; }

    public Booking()
    {
        Guests = new HashSet<Guest>();
        Payments = new HashSet<Payment>();
    }


    // Domain Behavior
    public void Cancel()
    {
        // Rule: Cannot cancel if less than 3 days remain to check-in
        if (Period.Start.AddDays(-3) < DateTime.Now)
        {
            // Ideally, throw a specific DomainException here
            throw new Exception("Cancellation is not allowed within 3 days of check-in.");
        }

        if (Status == BookingStatus.Confirmed || Status == BookingStatus.Pending)
        {
            Status = BookingStatus.Cancelled;
            // TODO: Raise Domain Event (e.g., BookingCancelledEvent)
        }
    }
}
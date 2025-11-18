using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class Guest : Entity<Guid>
{
    public Guid BookingId { get; set; } // Hangi rezervasyona ait?

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; } // Çocuk/Yetişkin ayrımı ve yasal zorunluluk
    public string Nationality { get; set; }   // İstatistik ve yasal bildirim için

    // Bu kişi "odadan sorumlu" esas kişi mi?
    public bool IsPrimary { get; set; }

    // İlişki
    public virtual Booking Booking { get; set; }
}


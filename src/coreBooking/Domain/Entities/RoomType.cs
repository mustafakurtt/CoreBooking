using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class RoomType : Entity<Guid>
{
    public Guid HotelId { get; set; } // Hangi otele ait
    public string Name { get; set; }    // Örn: "Standart Oda", "Kral Dairesi"
    public int Capacity { get; set; }   // Kaç kişilik (pax)

    // İlişkiler
    public virtual Hotel Hotel { get; set; }
    public virtual ICollection<Inventory> Inventories { get; set; }
    public virtual ICollection<Booking> Bookings { get; set; }

    public RoomType()
    {
        Inventories = new HashSet<Inventory>();
        Bookings = new HashSet<Booking>();
    }
}
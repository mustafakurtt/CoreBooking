using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class Hotel : Entity<Guid>
{
    public string Name { get; set; }
    public string City { get; set; }
    public string Address { get; set; }

    // İlişki: Bir otelin birden fazla oda tipi olabilir
    public virtual ICollection<RoomType> RoomTypes { get; set; }

    public Hotel()
    {
        RoomTypes = new HashSet<RoomType>();
    }
}
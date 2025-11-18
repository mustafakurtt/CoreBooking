using Domain.ValueObjects;
using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class Inventory : Entity<Guid>
{
    public Guid RoomTypeId { get; set; } // Hangi oda tipine ait
    public DateTime Date { get; set; }     // Hangi günün envanteri (Günlük tutacağız)
    public int Quantity { get; set; }      // O gün için satılabilir oda sayısı
    public Money Price { get; set; }

    // Concurrency Token: To prevent race conditions (Overbooking)
    public byte[] RowVersion { get; set; }

    // Relationship
    public virtual RoomType RoomType { get; set; }
}
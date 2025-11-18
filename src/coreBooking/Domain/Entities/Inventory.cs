using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class Inventory : Entity<Guid>
{
    public Guid RoomTypeId { get; set; } // Hangi oda tipine ait
    public DateTime Date { get; set; }     // Hangi günün envanteri (Günlük tutacağız)
    public int Quantity { get; set; }      // O gün için satılabilir oda sayısı
    public decimal Price { get; set; }     // O günkü fiyatı

    // Concurrency (Eşzamanlılık) için RowVersion.
    // EF Core bunu bir "timestamp" sütunu olarak yapılandıracak.
    // "Son odayı" iki kişinin aynı anda almasını bu engelleyecek.
    public byte[] RowVersion { get; set; }

    // İlişki
    public virtual RoomType RoomType { get; set; }
}
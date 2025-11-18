using Domain.Enums;
using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class Booking : Entity<Guid>
{
    public Guid CustomerId { get; set; } // narchitecture User tablosuna referans
    public Guid RoomTypeId { get; set; }

    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int NumberOfGuests { get; set; }
    public decimal TotalPrice { get; set; }
    public BookingStatus Status { get; set; }

    //İlişkiler
    public virtual User Customer { get; set; }
    public virtual RoomType RoomType { get; set; }

    //Domain Mantığı(V2)
    // Bu metotları şimdiden buraya eklemek,
    // buranın bir "POCO" değil, "zengin" bir domain varlığı
    // olduğunu gösterir.
    public void Cancel()
    {
        // V2'de buraya "son 3 günden az kaldıysa iptal edemez"
        // gibi kurallar eklenecek.
        if (Status == BookingStatus.Confirmed || Status == BookingStatus.Pending)
        {
            Status = BookingStatus.Cancelled;
            // Bir "BookingCancelled" olayı (event) da yayınlanabilir.
        }
    }
}
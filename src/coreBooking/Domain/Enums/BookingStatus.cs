namespace Domain.Enums;

public enum BookingStatus
{
    Pending = 1,    // Rezervasyon oluşturuldu, ödeme bekleniyor/onaylanmadı
    Confirmed = 2,  // Onaylandı, oda rezerve edildi
    Cancelled = 3   // İptal edildi
}
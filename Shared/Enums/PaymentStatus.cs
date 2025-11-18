namespace Shared.Enums;

public enum PaymentStatus
{
    Pending = 1,   // Ödeme bekleniyor
    Completed = 2, // Başarılı (Para kasada)
    Failed = 3,    // Hata (Yetersiz bakiye vb.)
    Refunded = 4   // İade edildi (İptal senaryosu için kritik)
}
namespace Application.Features.Guests.Constants;

public static class GuestsBusinessMessages
{
    public const string SectionName = "Guests";

    public const string GuestNotExists = "GuestNotExists";
    public const string BookingNotExists = "BookingNotExists"; // Rezervasyon yoksa misafir eklenemez
    public const string BookingCapacityExceeded = "BookingCapacityExceeded"; // 2 kiþilik odaya 3. kiþi eklenemez
    public const string BookingNotActive = "BookingNotActive"; // Ýptal edilmiþ/bitmiþ rezervasyona iþlem yapýlamaz
}
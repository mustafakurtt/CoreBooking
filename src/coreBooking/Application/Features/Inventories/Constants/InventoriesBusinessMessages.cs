namespace Application.Features.Inventories.Constants;

public static class InventoriesBusinessMessages
{
    public const string SectionName = "Inventory";

    public const string InventoryNotExists = "InventoryNotExists";
    public const string RoomTypeNotExists = "RoomTypeNotExists";
    public const string InventoryAlreadyExists = "InventoryAlreadyExists"; // Ayný tarih ve oda için kayýt var
    public const string InventoryDateCannotBeInPast = "InventoryDateCannotBeInPast"; // Geçmiþe stok girilemez/güncellenemez
    public const string QuantityCannotBeNegative = "QuantityCannotBeNegative"; // "Miktar negatif olamaz"
    public const string PriceMustBeGreaterThanZero = "PriceMustBeGreaterThanZero"; // "Fiyat 0'dan büyük olmalý"
}
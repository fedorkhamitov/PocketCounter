namespace PocketCounter.Domain.Share;

/// <summary>
/// CreatedOnly - только что созданный заказ
/// Completed - заказ собран и готов к отправке
/// Reserved - забронировано
/// PartlyReserved - забронировано частично
/// Deferred - заказ собран, но отправка отложена
/// Shipped - заказ отправлен
/// </summary>
public enum OrderStatus
{
    None,
    CreatedOnly,
    Completed,
    Reserved,
    PartlyReserved,
    Deferred,
    Shipped
}
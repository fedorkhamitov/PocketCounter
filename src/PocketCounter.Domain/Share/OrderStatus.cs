namespace PocketCounter.Domain.Share;

/// <summary>
/// CreatedOnly - только что созданный заказ
/// Completed - заказ собран и готов к отправке
/// PartiallyCompleted - часть заказа собрана и заказ отложен до готовности к полной комплектации
/// Deferred - заказ собран, но отправка отложена
/// Shipped - заказ отправлен
/// </summary>
public enum OrderStatus
{
    None,
    CreatedOnly,
    Completed,
    PartiallyCompleted,
    Deferred,
    Shipped
}
using CSharpFunctionalExtensions;
using PocketCounter.Domain.Share;

namespace PocketCounter.Domain.ValueObjects;

/// <summary>
///  <properties>
///     QuantityForShipping: Количество товара на отправку.
///     ReservedQuantity: Количество товара, забронированного.
///     ActualQuantity: Фактическое количество товара на складе.
///     AvailableForSale: Количество товара, доступное для продажи, вычисляется как разница между фактическим количеством 
///         и суммой забронированного и отправляемого товара.
/// </properties>
/// <constructor>
///     Проверяет входные параметры на корректность (отрицательные значения и превышение фактического количества).
/// </constructor> 
/// <methods>
///     ToString() : Возвращает строковое представление объекта.
///     Переопределены методы Equals и GetHashCode для корректного сравнения экземпляров класса.
/// </methods>
/// </summary>
public class ProductQuantity : ValueObject
{
    public int QuantityForShipping { get; }
    public int ReservedQuantity { get; }
    public int ActualQuantity { get; }
    public int AvailableForSale => ActualQuantity - (ReservedQuantity + QuantityForShipping);

    private ProductQuantity(int quantityForShipping, int reservedQuantity, int actualQuantity)
    {
        QuantityForShipping = quantityForShipping;
        ReservedQuantity = reservedQuantity;
        ActualQuantity = actualQuantity;
    }

    public override string ToString()
    {
        return
            $"Товар: На отправку: {QuantityForShipping}, Бронь: {ReservedQuantity}, На складе: {ActualQuantity}, Доступно для продажи: {AvailableForSale}";
    }

    // Переопределение Equals и GetHashCode для корректного сравнения экземпляров Value Object
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return QuantityForShipping;
        yield return ReservedQuantity;
        yield return ActualQuantity;
        yield return AvailableForSale;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not ProductQuantity other)
            return false;

        return QuantityForShipping == other.QuantityForShipping &&
               ReservedQuantity == other.ReservedQuantity &&
               ActualQuantity == other.ActualQuantity;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(QuantityForShipping, ReservedQuantity, ActualQuantity);
    }

    public static Result<ProductQuantity, Error> Create(int quantityForShipping, int reservedQuantity,
        int actualQuantity)
    {
        if (quantityForShipping < 0)
            return Errors.General.ValueIsRequired("Quantity For Shipping");
        if (reservedQuantity < 0)
            return Errors.General.ValueIsRequired("Reserved Quantity");
        if (actualQuantity < 0)
            return Errors.General.ValueIsRequired("Actual Quantity");

        return new ProductQuantity(quantityForShipping, reservedQuantity, actualQuantity);
    }
}
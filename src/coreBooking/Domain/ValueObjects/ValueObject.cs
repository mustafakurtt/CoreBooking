using System.Collections;

namespace Domain.ValueObjects;

// Bu sınıf, Value Object'lerin ana tanımıdır.
// Tüm Value Object'lerimiz (Address, Money) bu sınıftan miras alacak.
public abstract class ValueObject
{
    // Bir Value Object'in eşitliğini belirleyen özellikleri listeler.
    // Örn: Money için Amount ve Currency listelenir.
    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (GetType() != obj.GetType()) return false;

        // Gelen nesneyi ValueObject olarak deneriz
        var other = (ValueObject)obj;

        // GetEqualityComponents listeleri aynı ise, nesneler de aynıdır.
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        // Eşitlik bileşenlerinin (GetEqualityComponents) hash kodlarını birleştirir.
        return GetEqualityComponents()
            .Select(x => x != null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);
    }

    // DDD'de zorunlu olan == ve != operatörlerini de tanımlarız.
    public static bool operator ==(ValueObject left, ValueObject right)
    {
        if (left is null ^ right is null) return false;
        return left is null || left.Equals(right);
    }

    public static bool operator !=(ValueObject left, ValueObject right)
    {
        return !(left == right);
    }
}
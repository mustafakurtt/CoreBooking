using Shared.Constants;
using Shared.Enums;

namespace Domain.ValueObjects;

public class Money : ValueObject
{
    public decimal Amount { get; private set; }
    public Currency Currency { get; private set; }

    private Money() { }

    public Money(decimal amount, Currency currency)
    {
        if (amount < 0)
            throw new ArgumentException(ExceptionMessages.MoneyAmountCannotBeNegative, nameof(amount));

        if (!Enum.IsDefined(typeof(Currency), currency))
            throw new ArgumentException(ExceptionMessages.InvalidCurrencyType, nameof(currency));

        Amount = amount;
        Currency = currency;
    }

    public static Money operator +(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new ArgumentException(ExceptionMessages.CurrenciesMustMatchForOperation);

        return new Money(a.Amount + b.Amount, a.Currency);
    }

    public static Money operator -(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new ArgumentException(ExceptionMessages.CurrenciesMustMatchForOperation);

        return new Money(a.Amount - b.Amount, a.Currency);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    public override string ToString() => $"{Amount:N2} {Currency}";
}
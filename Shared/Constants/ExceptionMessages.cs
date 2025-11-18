namespace Shared.Constants;

public static class ExceptionMessages
{
    // Money & Currency
    public const string MoneyAmountCannotBeNegative = "Money amount cannot be negative.";
    public const string InvalidCurrencyType = "Invalid currency type provided.";
    public const string CurrenciesMustMatchForOperation = "Currencies must match for this operation.";

    // Address
    public const string AddressFieldEmpty = "{0} field cannot be empty."; // {0}: Street, City etc.
    public const string AddressFieldTooLong = "{0} field cannot be longer than {1} characters.";

    // DateRange
    public const string EndDateMustBeGreaterThanStartDate = "End date must be greater than start date.";

    // Booking
    public const string CancellationDeadlinePassed = "Cancellation is not allowed within {0} days of check-in.";
}
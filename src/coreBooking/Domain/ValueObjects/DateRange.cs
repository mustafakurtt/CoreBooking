using Shared.Constants;

namespace Domain.ValueObjects;

public class DateRange : ValueObject
{
    public DateTime Start { get; private set; }
    public DateTime End { get; private set; }

    private DateRange() { }

    public DateRange(DateTime start, DateTime end)
    {
        if (end <= start)
            throw new ArgumentException(ExceptionMessages.EndDateMustBeGreaterThanStartDate, nameof(end));

        Start = start.Date;
        End = end.Date;
    }

    public int Days => (End - Start).Days;

    public bool Overlaps(DateRange other)
    {
        return Start < other.End && other.Start < End;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Start;
        yield return End;
    }

    public override string ToString() => $"{Start:dd.MM.yyyy} - {End:dd.MM.yyyy} ({Days} Nights)";
}
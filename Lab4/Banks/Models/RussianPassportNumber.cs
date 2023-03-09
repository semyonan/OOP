using Banks.Exceptions;

namespace Banks.Models;

public class RussianPassportNumber : IEquatable<RussianPassportNumber>
{
    private const int MaxPassportNumber = 999999;
    private const int MinPassportNumber = 100000;
    private const int MaxSeries = 9999;
    private const int MinSeries = 1000;
    public RussianPassportNumber(int series, int number)
    {
        if ((series < MinSeries)
             || (series > MaxSeries)
             || (number < MinPassportNumber)
             || (number > MaxPassportNumber))
        {
            throw new BankValidationException("Invalid Passport Number arguments");
        }

        Series = series;
        Number = number;
    }

    public int Series { get; }
    public int Number { get; }

    public bool Equals(RussianPassportNumber? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Series == other.Series && Number == other.Number;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((RussianPassportNumber)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Series, Number);
    }
}
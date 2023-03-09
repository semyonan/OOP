using Banks.Exceptions;

namespace Banks.Models;

public class Money : IComparable<Money>
{
    public Money(decimal sum)
    {
        if (sum < 0)
        {
            throw new BankValidationException("Invalid money arguments");
        }

        Sum = sum;
    }

    public decimal Sum { get; }

    public static Money operator *(Money m1, decimal d)
    {
        return new Money(m1.Sum * d);
    }

    public static Money operator +(Money m1, Money m2)
    {
        return new Money(m1.Sum + m2.Sum);
    }

    public static bool operator <(Money m1, Money m2)
    {
        return m1.Sum < m2.Sum;
    }

    public static bool operator >(Money m1, Money m2)
    {
        return m1.Sum > m2.Sum;
    }

    public static bool operator ==(Money? m1, Money? m2)
    {
        return m1?.Sum == m2?.Sum;
    }

    public static bool operator !=(Money? m1, Money? m2)
    {
        return m1?.Sum != m2?.Sum;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Money)obj);
    }

    public override int GetHashCode()
    {
        return Sum.GetHashCode();
    }

    public int CompareTo(Money? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return Sum.CompareTo(other.Sum);
    }

    protected bool Equals(Money other)
    {
        return Sum == other.Sum;
    }
}
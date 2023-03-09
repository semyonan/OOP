using Shops.Exceptions;

namespace Shops.Models;

public class Price : IComparable
{
    public Price(decimal priceValue)
    {
        if (priceValue < 0)
        {
            throw new PriceException("Price of product can't be under zero");
        }

        Value = priceValue;
    }

    public decimal Value { get; }

    public static bool operator >(Price p1, Price p2)
    {
        return p1.Value > p2.Value;
    }

    public static bool operator <(Price p1, Price p2)
    {
        return p1.Value < p2.Value;
    }

    public static Price operator +(Price p, decimal toAdd)
    {
        return new Price(p.Value + toAdd);
    }

    public static Price operator -(Price p, decimal toSub)
    {
        return new Price(p.Value - toSub);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj is Price price)
        {
            return price.Value == Value;
        }

        return false;
    }

    public int CompareTo(object? obj)
    {
        if (obj is not Price price)
        {
            throw new PriceException("Invalid argument");
        }

        return Value.CompareTo(price.Value);
    }
}
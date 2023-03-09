using Banks.Exceptions;

namespace Banks.Models;

public class Percent
{
    private const double MaxPercentValue = 100;
    private const double MinPercentValue = 0;
    public Percent(double value)
    {
        if (value is <= MinPercentValue or >= MaxPercentValue)
        {
            throw new BankValidationException("Invalid percent arguments");
        }

        Value = value / 100;
    }

    public double Value { get; }

    public decimal AsDecimal()
    {
        return (decimal)Value;
    }

#pragma warning disable SA1201
    public static Percent operator /(Percent p1, int del)
    {
        if (del == 0)
        {
            throw new BankValidationException("Del must be more than a zero");
        }

        return new Percent(p1.Value * 100 / del);
    }

    public override string ToString()
    {
        return $"{Value}%";
    }
}
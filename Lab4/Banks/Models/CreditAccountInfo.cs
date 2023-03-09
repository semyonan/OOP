namespace Banks.Models;

public class CreditAccountInfo
{
    public CreditAccountInfo(Percent commission, Money limit)
    {
        Commission = commission;
        Limit = limit;
    }

    public Percent Commission { get; private set; }
    public Money Limit { get; private set; }

    public void ChangeCommission(Percent commission)
    {
        Commission = commission;
    }

    public void ChangeCommission(Money limit)
    {
        Limit = limit;
    }
}
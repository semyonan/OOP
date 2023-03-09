namespace Banks.Models;

public class DebitAccountInfo
{
    public DebitAccountInfo(Percent interests)
    {
        InterestsPerDay = interests / 365;
    }

    public Percent InterestsPerDay { get; private set; }

    public void ChangeInterests(Percent newInterests)
    {
        InterestsPerDay = newInterests / 365;
    }
}
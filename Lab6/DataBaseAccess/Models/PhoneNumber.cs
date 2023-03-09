namespace Reports.Service.Entities;

public class PhoneNumber
{
    public PhoneNumber(long number)
    {
        Number = number;
    }

    public long Number { get; }
}
using Banks.Exceptions;

namespace Banks.Models;

public class Address
{
    private const uint MaxNumberOfAddressComponents = 5;
    private const uint MinNumberOfAddressComponents = 4;
    private const uint MaxIndexNumber = 999999;
    private const uint MinIndexNumber = 100000;
    public Address(string address)
    {
        string[] subs = address.Split(',');
        if ((subs.Length < MinNumberOfAddressComponents)
            || (subs.Length > MaxNumberOfAddressComponents)
            || uint.Parse(subs[0]) > MaxIndexNumber
            || uint.Parse(subs[0]) < MinIndexNumber
            || string.IsNullOrEmpty(subs[1])
            || string.IsNullOrEmpty(subs[2])
            || string.IsNullOrEmpty(subs[3])
            || ((subs.Length == MaxNumberOfAddressComponents) && string.IsNullOrEmpty(subs[4])))
        {
            throw new BankValidationException("Invalid address arguments");
        }

        Index = uint.Parse(subs[0]);
        City = subs[1];
        Street = subs[2];
        Building = subs[3];
        if (subs.Length == MaxNumberOfAddressComponents)
        {
            Etc = subs[4];
        }
    }

    public uint Index { get; }
    public string City { get; }
    public string Street { get; }
    public string Building { get; }
    public string? Etc { get; }
}
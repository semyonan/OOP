using Banks.Exceptions;
using Banks.Models;
using Spectre.Console;

namespace Banks.Entities;

public class Client : IEquatable<Client>
{
    private readonly List<IBankAccount> _listOfAccounts;

    public Client(
        string name,
        string serName,
        Address? address = null,
        RussianPassportNumber? passportNumber = null,
        IMessageHandler? messageHandler = null)
    {
        if (string.IsNullOrWhiteSpace(name)
            || string.IsNullOrWhiteSpace(serName))
        {
            throw new BankValidationException("Invalid client arguments");
        }

        Name = name;
        SerName = serName;
        Address = address;
        PassportNumber = passportNumber;

        if (messageHandler == null)
        {
            MessageHandler = new EmptyMessageHandler();
        }
        else
        {
            MessageHandler = messageHandler;
        }

        Id = Guid.NewGuid();
        _listOfAccounts = new List<IBankAccount>();
    }

    public string Name { get; }
    public string SerName { get; }
    public Address? Address { get; private set; }

    public IMessageHandler MessageHandler { get; private set; }

    public Guid Id { get; }

    public IReadOnlyList<IBankAccount> ListOfAccounts => _listOfAccounts.AsReadOnly();
    public RussianPassportNumber? PassportNumber { get; private set; }

    public void AddAccount(IBankAccount bankAccount)
    {
        _listOfAccounts.Add(bankAccount);
    }

    public void AddAddress(Address address)
    {
        Address = address;
    }

    public void AddPassportNumber(RussianPassportNumber passportNumber)
    {
        if (PassportNumber != null)
        {
            throw new BankValidationException("You can't change passport number twice");
        }

        PassportNumber = passportNumber;
    }

    public bool Equals(Client? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id.Equals(other.Id) && Equals(PassportNumber, other.PassportNumber);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Client)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, PassportNumber);
    }
}
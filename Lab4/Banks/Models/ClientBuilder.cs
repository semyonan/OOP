using Banks.Entities;

namespace Banks.Models;

public class ClientBuilder
{
    private string? _name;
    private string? _serName;
    private Address? _address;
    private RussianPassportNumber? _passportNumber;
    private IMessageHandler? _messageHandler;

    public ClientBuilder()
    {
    }

    public ClientBuilder Name(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new Exception();
        }

        _name = name;

        return this;
    }

    public ClientBuilder SerName(string serName)
    {
        if (string.IsNullOrWhiteSpace(serName))
        {
            throw new Exception();
        }

        _serName = serName;

        return this;
    }

    public ClientBuilder Address(Address? addres)
    {
        _address = addres;

        return this;
    }

    public ClientBuilder PassportDate(RussianPassportNumber? passportNumber)
    {
        _passportNumber = passportNumber;

        return this;
    }

    public ClientBuilder MassageHandler(IMessageHandler? massageHandler)
    {
        _messageHandler = massageHandler;

        return this;
    }

    public Client Build()
    {
        if ((_name == null) || (_serName == null))
        {
            throw new Exception();
        }

        return new Client(_name, _serName, _address, _passportNumber, _messageHandler);
    }
}
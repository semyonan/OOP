namespace Shops.Exceptions;

public class AddressException : Exception
{
    public AddressException() { }
    public AddressException(string message)
        : base(message) { }
}
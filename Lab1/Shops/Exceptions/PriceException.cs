namespace Shops.Exceptions;

public class PriceException : Exception
{
    public PriceException() { }
    public PriceException(string message)
        : base(message) { }
}
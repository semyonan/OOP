namespace Shops.Exceptions;

public class ItemException : Exception
{
    public ItemException() { }
    public ItemException(string message)
        : base(message) { }
}
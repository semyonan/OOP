namespace Banks.Exceptions;

public class BankException : System.Exception
{
    public BankException() { }

    public BankException(string message)
        : base(message)
    {
    }
}
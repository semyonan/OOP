namespace Banks.Exceptions;

public class TransactionException : System.Exception
{
    public TransactionException() { }

    public TransactionException(string message)
        : base(message)
    {
    }
}
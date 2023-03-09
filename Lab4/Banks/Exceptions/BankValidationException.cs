namespace Banks.Exceptions;

public class BankValidationException : System.Exception
{
    public BankValidationException() { }

    public BankValidationException(string message)
        : base(message)
    {
    }
}
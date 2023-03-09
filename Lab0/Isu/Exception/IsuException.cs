namespace Isu.Exception;

public class IsuException : System.Exception
{
    public IsuException() { }
    public IsuException(string message)
        : base(message) { }
}
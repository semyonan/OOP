namespace Isu.Extra.Exceptions;

public class TimeException : System.Exception
{
    public TimeException() { }
    public TimeException(string message)
        : base(message) { }
}
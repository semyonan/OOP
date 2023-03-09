namespace Isu.Extra.Exceptions;

public class TimetableException : System.Exception
{
    public TimetableException() { }
    public TimetableException(string message)
        : base(message) { }
}
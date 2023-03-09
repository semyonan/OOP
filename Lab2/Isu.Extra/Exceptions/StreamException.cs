namespace Isu.Extra.Exceptions;

public class StreamException : System.Exception
{
    public StreamException() { }
    public StreamException(string message)
        : base(message) { }
}
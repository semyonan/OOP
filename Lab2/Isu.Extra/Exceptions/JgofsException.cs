namespace Isu.Extra.Exception;

public class JgofsException : System.Exception
{
    public JgofsException() { }
    public JgofsException(string message)
        : base(message) { }
}
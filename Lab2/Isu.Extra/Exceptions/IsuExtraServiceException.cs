namespace Isu.Extra.Exceptions;

public class IsuExtraServiceException : System.Exception
{
    public IsuExtraServiceException() { }
    public IsuExtraServiceException(string message)
        : base(message) { }
}
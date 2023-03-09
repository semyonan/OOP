namespace Backups.Exceptions;

public class BackupsException : System.Exception
{
    public BackupsException() { }
    public BackupsException(string message)
        : base(message) { }
}
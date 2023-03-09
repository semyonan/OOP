namespace Backups.Exceptions;

public class FileSystemException : System.Exception
{
    public FileSystemException() { }
    public FileSystemException(string message)
        : base(message) { }
}
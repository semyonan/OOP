namespace Backups.Extra.Exceptions;

public class BackupExtraException : System.Exception
{
    public BackupExtraException() { }
    public BackupExtraException(string message)
        : base(message) { }
}
namespace Backups.Entities;

public interface IBackupObject
{
    public string FullPathName { get; }
    public string Name { get; }
}
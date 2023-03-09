using Backups.Entities;

namespace Backups.Extra.Models.Saving;

public class BackupObjectContent
{
    public BackupObjectContent(IBackupObject backupObject)
    {
        Type = backupObject.GetType();
        BackupObject = backupObject;
    }

    public Type Type { get; }
    public IBackupObject BackupObject { get; }
}
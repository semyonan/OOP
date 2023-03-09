using Backups.Entities;
using Backups.Extra.Entities;

namespace Backups.Extra.Models.Restore;

public interface IUnzipedObject
{
    public void AcceptWriting(IExtraWritingRepository writingRepository, IBackupObject backupObject);
}
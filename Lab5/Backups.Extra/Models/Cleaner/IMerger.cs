using Backups.Entities;
using Backups.Extra.Entities;
using Backups.Models;

namespace Backups.Extra.Models.Cleaner;

public interface IMerger
{
    public List<IBackupObject> MergeBackupObjects(RestorePointInfo oldPoint, RestorePointInfo newPoint, IExtraWritingRepository writingRepository);
    public List<BackupZipArchive> MergeBackupZipArchives(RestorePointInfo oldPoint, RestorePointInfo newPoint, IExtraWritingRepository writingRepository);
}
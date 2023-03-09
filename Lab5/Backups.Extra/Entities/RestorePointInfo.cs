using Backups.Entities;
using Backups.Extra.Models.Restore;

namespace Backups.Extra.Entities;

public class RestorePointInfo
{
    public RestorePointInfo(RestorePoint restorePoint, IRestorationStorageAlgorithm storageAlgorithm)
    {
        RestorePoint = restorePoint;
        StorageAlgorithm = storageAlgorithm;
    }

    public RestorePoint RestorePoint { get; }
    public IRestorationStorageAlgorithm StorageAlgorithm { get; }
}
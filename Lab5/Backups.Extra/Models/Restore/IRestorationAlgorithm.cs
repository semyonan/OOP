using Backups.Entities;

namespace Backups.Extra.Models.Restore;

public interface IRestorationStorageAlgorithm : IStorageAlgorithm
{
    public Unstorage Restore(RestorePoint restorePoint);
}
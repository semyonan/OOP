using Backups.Entities;

namespace Backups.Extra.Models.Restore;

public interface IFileSystemRestorationStorageAlgorithm : IRestorationStorageAlgorithm, IFileSystemStorageAlgorithm
{
}
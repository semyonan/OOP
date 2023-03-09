using Backups.Entities;
using Backups.Models;

namespace Backups.Extra.Entities;

[Serializable]
public class InitialFileSystemRepositoryConfiguration : IInitialRepositoryConfiguration
{
    public InitialFileSystemRepositoryConfiguration(
        FileSystemReadingRepository fileSystemReadingRepository,
        ExtraFileSystemWritingRepository fileSystemWritingRepository)
    {
        ReadingRepository = fileSystemReadingRepository;
        ExtraWritingRepository = fileSystemWritingRepository;
    }

    public IReadingRepository ReadingRepository { get; }
    public IExtraWritingRepository ExtraWritingRepository { get; }
}
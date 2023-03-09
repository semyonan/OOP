using Backups.Entities;

namespace Backups.Extra.Entities;

public interface IInitialRepositoryConfiguration
{
    public IReadingRepository ReadingRepository { get; }
    public IExtraWritingRepository ExtraWritingRepository { get; }
}
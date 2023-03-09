using Backups.Entities;
using Backups.Models;

namespace Backups.Extra.Entities;

public class InitialInMemoryRepositoryConfiguration : IInitialRepositoryConfiguration
{
    public InitialInMemoryRepositoryConfiguration(
        InMemoryReadingRepository readingRepository,
        InMemoryExtraWritingRepository extraWritingRepository)
    {
        ReadingRepository = readingRepository;
        ExtraWritingRepository = extraWritingRepository;
    }

    public IReadingRepository ReadingRepository { get; }
    public IExtraWritingRepository ExtraWritingRepository { get; }
}
namespace Backups.Entities;

public class BackupTaskConfiguration
{
    public BackupTaskConfiguration(IReadingRepository readingRepository, IWritingRepository writingRepository)
    {
        ReadingRepository = readingRepository;
        WritingRepository = writingRepository;
    }

    public IWritingRepository WritingRepository { get; }
    public IReadingRepository ReadingRepository { get; }
}
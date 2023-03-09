using System.Globalization;

namespace Backups.Entities;

public interface IStorageAlgorithm
{
    public Storage CreateStorage(
        List<IBackupObject> listOfBackupObjects,
        IReadingRepository readingRepository,
        uint version);
}
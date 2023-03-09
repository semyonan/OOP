using System.IO.Compression;
using Backups.Entities;

namespace Backups.Models;

public class SingleStorageAlgorithm : IFileSystemStorageAlgorithm
{
    public Storage CreateStorage(
        List<IBackupObject> listOfBackupObjects,
        IReadingRepository readingRepository,
        uint version)
    {
        var zipArchive = new BackupZipArchive($"Version_{version}.zip", listOfBackupObjects, readingRepository);
        var storage = new Storage(new List<BackupZipArchive> { zipArchive });

        return storage;
    }
}
using Backups.Entities;

namespace Backups.Models;

public class SplitStorageAlgorithm : IFileSystemStorageAlgorithm
{
    public Storage CreateStorage(
        List<IBackupObject> listOfBackupObjects,
        IReadingRepository readingRepository,
        uint version)
    {
        var archivesList = new List<BackupZipArchive>();
        foreach (IBackupObject backupObject in listOfBackupObjects)
        {
            int sameBackupObjectsNameCount = 0;
            string archiveName = $"{backupObject.Name}.zip";
            while (archivesList.Exists(x => x.Name == archiveName))
            {
                sameBackupObjectsNameCount++;
                archiveName =
                    $"{backupObject.Name}_{sameBackupObjectsNameCount}.zip";
            }

            var zipArchive = new BackupZipArchive(archiveName, new List<IBackupObject> { backupObject }, readingRepository);
            archivesList.Add(zipArchive);
        }

        var storage = new Storage(archivesList);

        return storage;
    }
}
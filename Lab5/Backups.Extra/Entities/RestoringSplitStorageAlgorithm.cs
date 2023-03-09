using Backups.Entities;
using Backups.Extra.Exceptions;
using Backups.Extra.Models.Restore;
using Backups.Models;

namespace Backups.Extra.Entities;

public class RestoringSplitStorageAlgorithm : SplitStorageAlgorithm, IFileSystemRestorationStorageAlgorithm
{
    public Unstorage Restore(RestorePoint restorePoint)
    {
        var backupObjectsRezipedFiles = new List<Tuple<IBackupObject, IUnzipedObject>>();

        if (restorePoint.Storage == null) return new Unstorage(backupObjectsRezipedFiles);

        var objectsAndItsZips = GetObjectsAndItsZips(
            restorePoint.ListOfBackupObjects,
            restorePoint.Storage.ListOfZipArchives);

        foreach (var objectAndItsZip in objectsAndItsZips)
        {
            if (objectAndItsZip.Item1 is FileBackupObject)
            {
                backupObjectsRezipedFiles.Add(
                    new Tuple<IBackupObject, IUnzipedObject>(
                        objectAndItsZip.Item1,
                        new UnzipedFileObject(objectAndItsZip.Item1.Name, objectAndItsZip.Item2)));
            }

            if (objectAndItsZip.Item1 is FolderBackupObject)
            {
                backupObjectsRezipedFiles.Add(
                    new Tuple<IBackupObject, IUnzipedObject>(
                        objectAndItsZip.Item1,
                        new UnzipedFolderObject(objectAndItsZip.Item1.Name, objectAndItsZip.Item2)));
            }
        }

        return new Unstorage(backupObjectsRezipedFiles);
    }

    private List<Tuple<IBackupObject, BackupZipArchive>> GetObjectsAndItsZips(
        IReadOnlyList<IBackupObject> listOfBackupObjects,
        IReadOnlyList<BackupZipArchive> backupZipArchives)
    {
        var objectsAndItsZips = new List<Tuple<IBackupObject, BackupZipArchive>>();

        foreach (IBackupObject backupObject in listOfBackupObjects)
        {
            int sameBackupObjectsNameCount = 0;
            string archiveName = $"{backupObject.Name}.zip";
            while (objectsAndItsZips.Exists(x => x.Item2.Name == archiveName))
            {
                sameBackupObjectsNameCount++;
                archiveName =
                    $"{backupObject.Name}_{sameBackupObjectsNameCount}.zip";
            }

            BackupZipArchive? backupZipArchive = backupZipArchives.FirstOrDefault(x => x.Name == archiveName);

            if (backupZipArchive == null)
            {
                throw new BackupExtraException("There is no zip archive with such name");
            }

            objectsAndItsZips.Add(new Tuple<IBackupObject, BackupZipArchive>(backupObject, backupZipArchive));
        }

        return objectsAndItsZips;
    }
}
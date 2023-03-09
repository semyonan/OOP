using Backups.Entities;
using Backups.Extra.Exceptions;
using Backups.Extra.Models.Restore;
using Backups.Models;

namespace Backups.Extra.Entities;

public class RestoringSingleStorageAlgorithm : SingleStorageAlgorithm, IFileSystemRestorationStorageAlgorithm
{
    public Unstorage Restore(RestorePoint restorePoint)
    {
        var backupObjectsUnzipedFiles = new List<Tuple<IBackupObject, IUnzipedObject>>();

        if (restorePoint.Storage == null) return new Unstorage(backupObjectsUnzipedFiles);

        var unzipedFolder = new UnzipedFolderObject(restorePoint.Storage.ListOfZipArchives[0].Name, restorePoint.Storage.ListOfZipArchives[0]);

        var objectsAndItsZips = GetObjectsAndItsZips(
            restorePoint.ListOfBackupObjects,
            unzipedFolder);

        foreach (var objectAndItsZip in objectsAndItsZips)
        {
            if (objectAndItsZip.Item1 is FileBackupObject)
            {
                var unzipedFile = unzipedFolder.ListOfUnzipedFiles.FirstOrDefault(x => x.Name == objectAndItsZip.Item2);
                if (unzipedFile == null) throw new BackupExtraException();
                backupObjectsUnzipedFiles.Add(
                    new Tuple<IBackupObject, IUnzipedObject>(
                        objectAndItsZip.Item1,
                        new UnzipedFileObject(unzipedFile.Data, objectAndItsZip.Item1.Name)));
            }

            if (objectAndItsZip.Item1 is FolderBackupObject folder)
            {
                var dir = unzipedFolder.ListOfUnzipedFolders.Where(x => x.StartsWith($"{folder.Name}") && (x.Count(c => c == '/') > 1)).ToList();
                var files = unzipedFolder.ListOfUnzipedFiles
                    .Where(x => x.Name.StartsWith($"{folder.Name}")).ToList();
                backupObjectsUnzipedFiles.Add(
                    new Tuple<IBackupObject, IUnzipedObject>(
                        objectAndItsZip.Item1,
                        new UnzipedFolderObject(files, dir, folder.Name)));
            }
        }

        return new Unstorage(backupObjectsUnzipedFiles);
    }

    private List<Tuple<IBackupObject, string>> GetObjectsAndItsZips(
        IReadOnlyList<IBackupObject> listOfBackupObjects,
        UnzipedFolderObject unzipedFolder)
    {
        var fullInfoUnzipedFile = unzipedFolder.ListOfUnzipedFiles
            .Where(x => !x.Name.Contains('/')).ToList();

        var unzipedFilesNames = fullInfoUnzipedFile
            .Select(x => x.Name)
            .ToList();

        var unzipedFolders = unzipedFolder.ListOfUnzipedFolders
            .Select(x => x.Split('/')[0]).ToList();

        var objectsAndItsZips = new List<Tuple<IBackupObject, string>>();

        foreach (IBackupObject backupObject in listOfBackupObjects)
        {
            int sameBackupObjectsNameCount = 0;
            string archiveName = $"{backupObject.Name}";
            while (objectsAndItsZips.Exists(x => x.Item2 == archiveName))
            {
                sameBackupObjectsNameCount++;
                archiveName =
                    $"{backupObject.Name}_{sameBackupObjectsNameCount}";
            }

            string? backupZipArchive = unzipedFolders.FirstOrDefault(x => x == archiveName);

            if (backupZipArchive == null)
            {
                backupZipArchive = unzipedFilesNames.FirstOrDefault(x => Path.GetFileNameWithoutExtension(x) == archiveName);
                if (backupZipArchive == null)
                {
                    throw new BackupExtraException("There is no zip archive with such name");
                }
            }

            objectsAndItsZips.Add(new Tuple<IBackupObject, string>(backupObject, backupZipArchive));
        }

        return objectsAndItsZips;
    }
}
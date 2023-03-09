using Backups.Entities;
using Backups.Extra.Models.Restore;
using Backups.Models;

namespace Backups.Extra.Entities;

public class ExtraFileSystemWritingRepository : FileSystemWritingRepository, IExtraWritingRepository
{
    private IFileSystemRestorationStorageAlgorithm _fileSystemRestorationStorageAlgorithm;
    public ExtraFileSystemWritingRepository(
        string fullPathName,
        IFileSystemRestorationStorageAlgorithm fileSystemRestorationStorageAlgorithm)
        : base(fullPathName, fileSystemRestorationStorageAlgorithm)
    {
        _fileSystemRestorationStorageAlgorithm = fileSystemRestorationStorageAlgorithm;
    }

    public IFileSystemRestorationStorageAlgorithm RestorationStorageAlgorithm => _fileSystemRestorationStorageAlgorithm;
    public void Add(Unstorage unbackupObject)
    {
        foreach (var obj in unbackupObject.ListOfObjectsAndItsRezipedFiles)
        {
            obj.Item2.AcceptWriting(this, obj.Item1);
        }
    }

    public void ChangeRestorationStorageAlgorithm(IRestorationStorageAlgorithm restorationStorageAlgorithm)
    {
        if (restorationStorageAlgorithm is IFileSystemRestorationStorageAlgorithm fileSystemRestorationStorageAlgorithm)
        {
            _fileSystemRestorationStorageAlgorithm = fileSystemRestorationStorageAlgorithm;
        }
    }

    public void AddRezipedFile(UnzipedFileObject file, IBackupObject backupObject)
    {
        if (File.Exists(backupObject.FullPathName))
        {
            File.Delete(backupObject.FullPathName);
        }

        File.WriteAllBytes(backupObject.FullPathName, file.File.Data);
    }

    public void AddRezipedFolder(UnzipedFolderObject folder, IBackupObject backupObject)
    {
        Directory.CreateDirectory(@$"/Users/anya/Documents/TestRestore/{folder.Name}");
        foreach (var dirName in folder.ListOfUnzipedFolders)
        {
            Directory.CreateDirectory(@$"/Users/anya/Documents/TestRestore/{dirName}");
        }

        foreach (var file in folder.ListOfUnzipedFiles)
        {
            File.WriteAllBytes(@$"/Users/anya/Documents/TestRestore/{file.Name}", file.Data);
        }
    }

    public void Delete(RestorePoint restorePoint, BackupZipArchive backupZipArchive)
    {
        File.Delete(Path.Combine(Name, restorePoint.Name, backupZipArchive.Name));
    }

    public void Delete(RestorePoint restorePoint)
    {
        Directory.Delete(Path.Combine(Name, restorePoint.Name), true);
    }

    public void Move(RestorePoint oldRestorePoint, BackupZipArchive backupZipArchive, RestorePoint newRestorePoint)
    {
       File.Move(
            Path.Combine(Name, oldRestorePoint.Name, backupZipArchive.Name),
            Path.Combine(Name, newRestorePoint.Name, backupZipArchive.Name));
    }

    public void Rename(RestorePoint restorePoint, string newName)
    {
        Directory.Move(
            Path.Combine(Name, restorePoint.Name),
            Path.Combine(Name, newName));
    }
}
using System.Net.Sockets;
using Backups.Entities;
using Backups.Extra.Models.Restore;
using Backups.Models;

namespace Backups.Extra.Entities;

public interface IExtraWritingRepository : IWritingRepository
{
    public string Name { get; }
    public IFileSystemRestorationStorageAlgorithm RestorationStorageAlgorithm { get; }
    public void Add(Unstorage unbackupObject);
    public void AddRezipedFile(UnzipedFileObject file, IBackupObject backupObject);
    public void AddRezipedFolder(UnzipedFolderObject folder, IBackupObject backupObject);
    public void Delete(RestorePoint restorePoint, BackupZipArchive backupZipArchive);
    public void Delete(RestorePoint restorePoint);
    public void Move(RestorePoint oldRestorePoint, BackupZipArchive backupZipArchive, RestorePoint newRestorePoint);
    public void Rename(RestorePoint restorePoint, string newName);
}
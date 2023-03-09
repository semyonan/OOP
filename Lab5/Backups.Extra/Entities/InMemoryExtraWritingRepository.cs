using Backups.Entities;
using Backups.Exceptions;
using Backups.Extra.Models.Restore;
using Backups.Models;

namespace Backups.Extra.Entities;

public class InMemoryExtraWritingRepository : InMemoryWritingRepository, IExtraWritingRepository
{
    private readonly List<InMemoryExtraWritingRepository> _repositoryComponentList;
    private IFileSystemRestorationStorageAlgorithm _fileSystemRestorationStorageAlgorithm;

    public InMemoryExtraWritingRepository(string name, IFileSystemRestorationStorageAlgorithm fileSystemStorageAlgorithm)
        : base(name, fileSystemStorageAlgorithm)
    {
        _repositoryComponentList = new List<InMemoryExtraWritingRepository>();
        _fileSystemRestorationStorageAlgorithm = fileSystemStorageAlgorithm;
    }

    public IFileSystemRestorationStorageAlgorithm RestorationStorageAlgorithm => _fileSystemRestorationStorageAlgorithm;

    public IReadOnlyList<InMemoryExtraWritingRepository> RepositoryExtraComponentList => _repositoryComponentList.AsReadOnly();
    public new IWritingRepository Add(RestorePoint restorePoint)
    {
        var inMemoryRepository = new InMemoryExtraWritingRepository(Path.Combine(Name, restorePoint.Name), _fileSystemRestorationStorageAlgorithm);
        _repositoryComponentList.Add(inMemoryRepository);

        return inMemoryRepository;
    }

    public new void Add(Storage storage)
    {
        foreach (var archive in storage.ListOfZipArchives)
        {
            var inMemoryRepository = new InMemoryExtraWritingRepository(Path.Combine(Name, archive.Name),  _fileSystemRestorationStorageAlgorithm);
            _repositoryComponentList.Add(inMemoryRepository);
        }
    }

    public new void ChangeStorageAlgorithm(IStorageAlgorithm storageAlgorithm)
    {
        if (storageAlgorithm is not IFileSystemRestorationStorageAlgorithm fileSystemStorageAlgorithm)
        {
            throw new FileSystemException("Algorithm is not correct for this repository");
        }

        _fileSystemRestorationStorageAlgorithm = fileSystemStorageAlgorithm;
    }

    public void Add(Unstorage unbackupObject)
    {
        foreach (var obj in unbackupObject.ListOfObjectsAndItsRezipedFiles)
        {
            obj.Item2.AcceptWriting(this, obj.Item1);
        }
    }

    public void AddRezipedFile(UnzipedFileObject file, IBackupObject backupObject)
    {
    }

    public void AddRezipedFolder(UnzipedFolderObject folder, IBackupObject backupObject)
    {
    }

    public void Delete(RestorePoint restorePoint, BackupZipArchive backupZipArchive)
    {
        var restorePointRepo = _repositoryComponentList.FirstOrDefault(x => x.Name == Path.Combine(Name, restorePoint.Name));

        restorePointRepo?.Delete(backupZipArchive);
    }

    public void Delete(RestorePoint restorePoint)
    {
        var restorePointRepo = _repositoryComponentList.FirstOrDefault(x => x.Name == Path.Combine(Name, restorePoint.Name));

        if (restorePointRepo != null)
        {
            _repositoryComponentList.Remove(restorePointRepo);
        }
    }

    public void Move(RestorePoint oldRestorePoint, BackupZipArchive backupZipArchive, RestorePoint newRestorePoint)
    {
        var oldRestorePointRepo = _repositoryComponentList.FirstOrDefault(x => x.Name == Path.Combine(Name, oldRestorePoint.Name));
        var newRestorePointRepo = _repositoryComponentList.FirstOrDefault(x => x.Name == Path.Combine(Name, newRestorePoint.Name));

        oldRestorePointRepo?.Delete(backupZipArchive);
        newRestorePointRepo?.Add(backupZipArchive);
    }

    public void Rename(RestorePoint restorePoint, string newName)
    {
        var restorePointRepo = _repositoryComponentList.FirstOrDefault(x => x.Name == Path.Combine(Name, restorePoint.Name));
        var components = restorePointRepo?.RepositoryExtraComponentList.ToList();

        var newNameRestorePointRepo =
            new InMemoryExtraWritingRepository(Path.Combine(Name, newName), RestorationStorageAlgorithm);

        _repositoryComponentList.Add(new InMemoryExtraWritingRepository(Path.Combine(Name, newName), RestorationStorageAlgorithm));

        if (components != null)
        {
            foreach (var component in components)
            {
                newNameRestorePointRepo._repositoryComponentList.Add(new InMemoryExtraWritingRepository(Path.Combine(Name, newName, Path.GetFileName(component.Name)), RestorationStorageAlgorithm));
                restorePointRepo?._repositoryComponentList.Remove(component);
            }
        }

        Delete(restorePoint);
    }

    private void Delete(BackupZipArchive backupZipArchive)
    {
        var objToDel = _repositoryComponentList.FirstOrDefault(x => x.Name == Path.Combine(Name, backupZipArchive.Name));

        if (objToDel != null)
        {
            _repositoryComponentList.Remove(objToDel);
        }
    }

    private void Add(BackupZipArchive backupZipArchive)
    {
        var inMemoryRepository = new InMemoryExtraWritingRepository(Path.Combine(Name, backupZipArchive.Name),  _fileSystemRestorationStorageAlgorithm);
        _repositoryComponentList.Add(inMemoryRepository);
    }
}
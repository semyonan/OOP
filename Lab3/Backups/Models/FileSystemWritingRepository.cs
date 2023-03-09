using Backups.Entities;
using Backups.Exceptions;

namespace Backups.Models;

public class FileSystemWritingRepository : IWritingRepository
{
    private IFileSystemStorageAlgorithm _fileSystemStorageAlgorithm;
    public FileSystemWritingRepository(string fullPathName, IFileSystemStorageAlgorithm fileSystemStorageAlgorithm)
    {
        if (string.IsNullOrWhiteSpace(fullPathName))
        {
            throw new BackupsException("Invalid arguments");
        }

        Name = fullPathName;
        _fileSystemStorageAlgorithm = fileSystemStorageAlgorithm;
    }

    public string Name { get; }
    public IStorageAlgorithm StorageAlgorithm => _fileSystemStorageAlgorithm;

    public IWritingRepository Add(RestorePoint restorePoint)
    {
        if (Directory.Exists(Path.Combine(Name, restorePoint.Name)))
        {
            throw new FileSystemException("Directory already exists");
        }

        Directory.CreateDirectory(Path.Combine(Name, restorePoint.Name));

        return new FileSystemWritingRepository(Path.Combine(Name, restorePoint.Name), _fileSystemStorageAlgorithm);
    }

    public void Add(Storage storage)
    {
        foreach (var archive in storage.ListOfZipArchives)
        {
            File.WriteAllBytes(Path.Combine(Name, archive.Name), archive.Data);
        }
    }

    public void ChangeStorageAlgorithm(IStorageAlgorithm storageAlgorithm)
    {
        if (storageAlgorithm is not IFileSystemStorageAlgorithm fileSystemStorageAlgorithm)
        {
            throw new FileSystemException("Algorithm is not correct for this repository");
        }

        _fileSystemStorageAlgorithm = fileSystemStorageAlgorithm;
    }
}
using Backups.Entities;
using Backups.Exceptions;

namespace Backups.Models;

public class InMemoryWritingRepository : IWritingRepository
{
    private readonly List<InMemoryWritingRepository> _repositoryComponentList;
    private IFileSystemStorageAlgorithm _fileSystemStorageAlgorithm;
    public InMemoryWritingRepository(string name, IFileSystemStorageAlgorithm fileSystemStorageAlgorithm)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BackupsException("Invalid arguments");
        }

        Name = name;
        _fileSystemStorageAlgorithm = fileSystemStorageAlgorithm;
        _repositoryComponentList = new List<InMemoryWritingRepository>();
    }

    public string Name { get; }
    public IReadOnlyList<InMemoryWritingRepository> RepositoryComponentList => _repositoryComponentList.AsReadOnly();
    public IStorageAlgorithm StorageAlgorithm => _fileSystemStorageAlgorithm;

    public IWritingRepository Add(RestorePoint restorePoint)
    {
        var inMemoryRepository = new InMemoryWritingRepository(Path.Combine(Name, restorePoint.Name), _fileSystemStorageAlgorithm);
        _repositoryComponentList.Add(inMemoryRepository);

        return inMemoryRepository;
    }

    public void Add(Storage storage)
    {
        foreach (var archive in storage.ListOfZipArchives)
        {
            var inMemoryRepository = new InMemoryWritingRepository(Path.Combine(Name, archive.Name),  _fileSystemStorageAlgorithm);
            _repositoryComponentList.Add(inMemoryRepository);
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
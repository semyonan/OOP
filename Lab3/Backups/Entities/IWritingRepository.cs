namespace Backups.Entities;

public interface IWritingRepository
{
    public IStorageAlgorithm StorageAlgorithm { get; }

    public IWritingRepository Add(RestorePoint restorePoint);
    public void Add(Storage storage);

    public void ChangeStorageAlgorithm(IStorageAlgorithm storageAlgorithm);
}
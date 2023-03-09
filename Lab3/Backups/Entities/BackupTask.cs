using Backups.Exceptions;

namespace Backups.Entities;

public class BackupTask
{
    private readonly List<IBackupObject> _listOfBackupObjects;
    private readonly List<RestorePoint> _listOfRestorePoints;

    public BackupTask(string name, BackupTaskConfiguration configuration)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new BackupsException("Invalid arguments");
        }

        Name = name;
        Configuration = configuration;
        _listOfBackupObjects = new List<IBackupObject>();
        _listOfRestorePoints = new List<RestorePoint>();
        VersionCount = 0;
    }

    public IReadOnlyList<IBackupObject> ListOfBackupObjects => _listOfBackupObjects.AsReadOnly();
    public IReadOnlyList<RestorePoint> ListOfRestorePoints => _listOfRestorePoints.AsReadOnly();
    public BackupTaskConfiguration Configuration { get; }
    public string Name { get; }
    public uint VersionCount { get; private set; }

    public void CreateRestorePoint()
    {
        VersionCount++;
        var curRestorePoint = new RestorePoint($"Restore_point_{VersionCount}_{DateTime.Now}", DateTime.Now, VersionCount, _listOfBackupObjects);
        var restorePointRepo = Configuration.WritingRepository.Add(curRestorePoint);

        Storage storage =
            Configuration.WritingRepository.StorageAlgorithm.CreateStorage(_listOfBackupObjects, Configuration.ReadingRepository, VersionCount);

        restorePointRepo.Add(storage);
        curRestorePoint.AddStorage(storage);

        _listOfRestorePoints.Add(curRestorePoint);
    }

    public void AddBackupObject(IBackupObject backupObject)
    {
        if (_listOfBackupObjects.Exists(x => x.FullPathName == backupObject.FullPathName))
        {
            throw new BackupsException("Backup object already exists");
        }

        _listOfBackupObjects.Add(backupObject);
    }

    public void RemoveBackupObject(IBackupObject backupObject)
    {
        var curBackupObject = _listOfBackupObjects.FirstOrDefault(x => x.FullPathName == backupObject.FullPathName);

        if (curBackupObject == null)
        {
            throw new BackupsException("Backup object doen't exists");
        }

        _listOfBackupObjects.Remove(curBackupObject);
    }

    public void ChangeStorageAlgorithm(IStorageAlgorithm storageAlgorithm)
    {
       Configuration.WritingRepository.ChangeStorageAlgorithm(storageAlgorithm);
    }

    public override string ToString()
    {
        return Name.ToString();
    }
}
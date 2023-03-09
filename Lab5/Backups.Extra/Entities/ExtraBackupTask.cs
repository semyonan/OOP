using Backups.Entities;
using Backups.Extra.Exceptions;
using Backups.Extra.Models.Cleaner;
using Backups.Extra.Models.Comparers;
using Backups.Extra.Models.Logger;
using Backups.Extra.Models.Restore;
using Backups.Extra.Models.Saving;
using Backups.Models;

namespace Backups.Extra.Entities;
public class ExtraBackupTask
{
    private readonly List<RestorePointInfo> _listOfRestorePointsInfo;
    private BackupTask _backupTask;
    public ExtraBackupTask(
        string name,
        IInitialRepositoryConfiguration repositoryConfiguration,
        RestorePointCleaner restorePointCleaner,
        IExtraWritingRepository extraWritingRepository,
        ILogger? logger = null)
    {
        _backupTask = new BackupTask(name, new BackupTaskConfiguration(repositoryConfiguration.ReadingRepository, extraWritingRepository));
        _listOfRestorePointsInfo = new List<RestorePointInfo>();
        RestorePointCleaner = restorePointCleaner;
        InitialRepositoryConfiguration = repositoryConfiguration;
        Logger = logger;
        Logger?.Log($"{name} backup task was created");
    }

    public ILogger? Logger { get; private set; }
    public RestorePointCleaner RestorePointCleaner { get; private set; }

    public IInitialRepositoryConfiguration InitialRepositoryConfiguration { get; }
    public IExtraWritingRepository WritingRepository => (_backupTask.Configuration.WritingRepository as IExtraWritingRepository) !;

    public IReadOnlyList<IBackupObject> ListOfBackupObjects => _backupTask.ListOfBackupObjects;
    public IReadOnlyList<RestorePointInfo> ListOfRestorePointsInfos => _listOfRestorePointsInfo.AsReadOnly();
    public string Name => _backupTask.Name;
    public uint VersionCount { get; set; }

    public RestorePointInfo GetRestorePointInfo(RestorePointInfo restorePointInfo) =>
        FindRestorePointInfo(restorePointInfo)
        ?? throw new BackupExtraException("Restore Point does't exists");

    public RestorePointInfo GetRestorePointInfo(RestorePoint restorePoint) =>
        FindRestorePointInfo(restorePoint)
        ?? throw new BackupExtraException("Restore Point does't exists");

    public void CreateRestorePoint()
    {
        _backupTask.CreateRestorePoint();
        _listOfRestorePointsInfo.Add(
            new RestorePointInfo(_backupTask.ListOfRestorePoints[^1], WritingRepository.RestorationStorageAlgorithm));
        VersionCount++;
        Logger?.Log($"Restore point {_backupTask.ListOfRestorePoints[^1].Name} was created");
    }

    public void CleanRestorePoints()
    {
        var pointsToClean = RestorePointCleaner.Select(ListOfRestorePointsInfos);

        RestorePointInfo? restorePointInfo = null;

        if (pointsToClean.Any())
        {
            var prevRestorePointInfo = pointsToClean[0].RestorePointInfo;

            for (var i = 1; i < pointsToClean.Count; i++)
            {
                restorePointInfo = RestorePointCleaner?.Merge(
                    prevRestorePointInfo,
                    pointsToClean[i].RestorePointInfo,
                    WritingRepository);

                _listOfRestorePointsInfo.Remove(GetRestorePointInfo(pointsToClean[i - 1].RestorePointInfo));
                Logger?.Log($"Restore point {pointsToClean[i - 1].RestorePointInfo.RestorePoint.Name} was merged");
            }

            AddMergedPoint(restorePointInfo);

            _listOfRestorePointsInfo.Remove(GetRestorePointInfo(pointsToClean[^1].RestorePointInfo));
        }
    }

    public void UnbackRestorePoint(RestorePoint restorePoint, IExtraWritingRepository? extraWritingRepository = null)
    {
        var restorePointInfo = GetRestorePointInfo(restorePoint);
        var writingRepository = InitialRepositoryConfiguration.ExtraWritingRepository;

        if (extraWritingRepository != null)
        {
            writingRepository = extraWritingRepository;
        }

        writingRepository
            .Add(restorePointInfo.StorageAlgorithm
                .Restore(GetRestorePointInfo(restorePoint).RestorePoint));

        RemoveRestorePoint(restorePointInfo.RestorePoint);

        Logger?.Log($"Restore point {restorePoint.Name} was restored into {writingRepository.Name}");
    }

    public void AddBackupObject(IBackupObject backupObject)
    {
        _backupTask.AddBackupObject(backupObject);
    }

    public void AddListOfBackupObjects(List<IBackupObject> list)
    {
        foreach (var backupObject in list)
        {
            AddBackupObject(backupObject);
        }
    }

    public void RemoveBackupObject(IBackupObject backupObject)
    {
        _backupTask.RemoveBackupObject(backupObject);
    }

    public void RemoveAllBackupObjects()
    {
        foreach (var backupObject in _backupTask.ListOfBackupObjects.ToList())
        {
            _backupTask.RemoveBackupObject(backupObject);
        }
    }

    public BackupTaskMemento Save(string destinationPath)
    {
        var memento = new BackupTaskMemento();
        memento.Serialize(this, destinationPath);

        Logger?.Log($"Backup task {Name} was saved into {destinationPath}");
        return memento;
    }

    public void Set(BackupTaskMemento backupTaskMemento)
    {
        var content = backupTaskMemento.Deserialize();

        if (content == null) return;

        _listOfRestorePointsInfo.Clear();
        _listOfRestorePointsInfo.AddRange(content.ListOfRestorePoints);

        VersionCount = content.VersionCount;

        RemoveAllBackupObjects();
        AddListOfBackupObjects(content.ListOfBackupObjects());

        Logger?.Log($"Backup task {Name} was set from {backupTaskMemento.ConfigurationPathName}");
    }

    public void ChangeStorageAlgorithm(IRestorationStorageAlgorithm storageAlgorithm)
    {
        _backupTask.ChangeStorageAlgorithm(storageAlgorithm);
    }

    public void ChangeRestorePointCleaner(RestorePointCleaner restorePointCleaner)
    {
        RestorePointCleaner = restorePointCleaner;
    }

    public void ChangeLogger(ILogger logger)
    {
        Logger = logger;
    }

    public override string ToString()
    {
        return _backupTask.ToString();
    }

    private void AddMergedPoint(RestorePointInfo? restorePointInfo)
    {
        if (restorePointInfo == null) return;

        var name = $"Restore_point_{restorePointInfo.RestorePoint.Version}_{DateTime.Now}";

        _listOfRestorePointsInfo.Insert(
            0,
            new RestorePointInfo(
            new RestorePoint(
                name,
                DateTime.Now,
                restorePointInfo.RestorePoint.Version,
                restorePointInfo.RestorePoint.ListOfBackupObjects.ToList()),
            restorePointInfo.StorageAlgorithm));

        if (restorePointInfo.RestorePoint.Storage != null)
        {
            _listOfRestorePointsInfo[^1].RestorePoint
                .AddStorage(
                    new Storage(new List<BackupZipArchive>(restorePointInfo.RestorePoint.Storage.ListOfZipArchives.ToList())));
        }

        Logger?.Log($"Merged restore point {name} was created");
    }

    private void RemoveRestorePoint(RestorePoint restorePoint)
    {
        _listOfRestorePointsInfo.Remove(GetRestorePointInfo(restorePoint));
        WritingRepository.Delete(restorePoint);
    }

    private RestorePointInfo? FindRestorePointInfo(RestorePointInfo restorePointInfo) => _listOfRestorePointsInfo
        .FirstOrDefault(x => new RestorePointComparer().Equals(x.RestorePoint, restorePointInfo.RestorePoint));

    private RestorePointInfo? FindRestorePointInfo(RestorePoint restorePoint) => _listOfRestorePointsInfo
        .FirstOrDefault(x => new RestorePointComparer().Equals(x.RestorePoint, restorePoint));
}
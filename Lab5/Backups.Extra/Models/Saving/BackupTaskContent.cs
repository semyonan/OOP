using Backups.Entities;
using Backups.Extra.Entities;

namespace Backups.Extra.Models.Saving;

public class BackupTaskContent
{
    private readonly List<BackupObjectContent> _backupObjectContents = new List<BackupObjectContent>();
    private readonly List<RestorePointInfo> _restorePointInfos = new List<RestorePointInfo>();
    public BackupTaskContent(ExtraBackupTask extraBackupTask)
    {
        foreach (var backupObject in extraBackupTask.ListOfBackupObjects)
        {
            _backupObjectContents.Add(new BackupObjectContent(backupObject));
        }

        _restorePointInfos.AddRange(extraBackupTask.ListOfRestorePointsInfos);
        VersionCount = extraBackupTask.VersionCount;
    }

    public BackupTaskContent()
    {
    }

    public IReadOnlyList<BackupObjectContent> ListOfBackupObjectsContents => _backupObjectContents.AsReadOnly();

    public IReadOnlyList<RestorePointInfo> ListOfRestorePoints => _restorePointInfos.AsReadOnly();
    public uint VersionCount { get; set; }

    public void AddBackupObjectsList(List<IBackupObject> backupObjectsList)
    {
        _backupObjectContents.Clear();
        foreach (var backupObject in backupObjectsList)
        {
            _backupObjectContents.Add(new BackupObjectContent(backupObject));
        }
    }

    public void AddRestorePointsList(List<RestorePointInfo> restorePointInfos)
    {
        _restorePointInfos.Clear();
        _restorePointInfos.AddRange(new List<RestorePointInfo>(restorePointInfos));
    }

    public List<IBackupObject> ListOfBackupObjects()
    {
        var listOfBackupObjects = new List<IBackupObject>();

        foreach (var content in _backupObjectContents)
        {
            listOfBackupObjects.Add(content.BackupObject);
        }

        return listOfBackupObjects;
    }
}
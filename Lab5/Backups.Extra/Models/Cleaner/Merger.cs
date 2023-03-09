using Backups.Entities;
using Backups.Extra.Entities;
using Backups.Extra.Models.Comparers;
using Backups.Models;

namespace Backups.Extra.Models.Cleaner;

public class Merger : IMerger
{
    public List<IBackupObject> MergeBackupObjects(RestorePointInfo oldPoint, RestorePointInfo newPoint, IExtraWritingRepository writingRepository)
    {
        var merged = new List<IBackupObject>();
        if (oldPoint.StorageAlgorithm is not SingleStorageAlgorithm)
        {
            merged.AddRange(MergeIntersectObjects(oldPoint, newPoint, writingRepository));
            merged.AddRange(MergeExceptObjects(oldPoint, newPoint, writingRepository));
        }

        return merged;
    }

    public List<BackupZipArchive> MergeBackupZipArchives(RestorePointInfo oldPoint, RestorePointInfo newPoint, IExtraWritingRepository writingRepository)
    {
        var merged = new List<BackupZipArchive>();
        if (oldPoint.StorageAlgorithm is not SingleStorageAlgorithm)
        {
            merged.AddRange(MergeIntersectArchives(oldPoint, newPoint, writingRepository));
            merged.AddRange(MergeExceptArchives(oldPoint, newPoint, writingRepository));
        }

        return merged;
    }

    private List<BackupZipArchive> MergeIntersectArchives(RestorePointInfo oldPoint, RestorePointInfo newPoint, IExtraWritingRepository writingRepository)
    {
        if (newPoint.RestorePoint.Storage == null || oldPoint.RestorePoint.Storage == null) return new List<BackupZipArchive>();

        var intersectObjectsFromOld = oldPoint.RestorePoint.Storage.ListOfZipArchives
            .Intersect(newPoint.RestorePoint.Storage.ListOfZipArchives, new ZipArchivesComparer())
            .ToList();

        var intersectObjectsFromNew = newPoint.RestorePoint.Storage.ListOfZipArchives
            .Intersect(oldPoint.RestorePoint.Storage.ListOfZipArchives, new ZipArchivesComparer())
            .ToList();

        for (int i = 0; i < intersectObjectsFromOld.Count; i++)
        {
            writingRepository.Delete(newPoint.RestorePoint, intersectObjectsFromNew[i]);
            writingRepository.Move(oldPoint.RestorePoint, intersectObjectsFromOld[i], newPoint.RestorePoint);
        }

        return intersectObjectsFromOld;
    }

    private List<BackupZipArchive> MergeExceptArchives(RestorePointInfo oldPoint, RestorePointInfo newPoint, IExtraWritingRepository writingRepository)
    {
        if (newPoint.RestorePoint.Storage == null || oldPoint.RestorePoint.Storage == null) return new List<BackupZipArchive>();

        var exceptObjects = oldPoint.RestorePoint.Storage.ListOfZipArchives
            .Except(newPoint.RestorePoint.Storage.ListOfZipArchives, new ZipArchivesComparer())
            .ToList();

        foreach (BackupZipArchive zip in exceptObjects)
        {
            writingRepository.Move(oldPoint.RestorePoint, zip, newPoint.RestorePoint);
        }

        return exceptObjects;
    }

    private List<IBackupObject> MergeIntersectObjects(RestorePointInfo oldPoint, RestorePointInfo newPoint, IExtraWritingRepository writingRepository)
    {
        if (newPoint.RestorePoint.Storage == null || oldPoint.RestorePoint.Storage == null) return new List<IBackupObject>();

        var intersectObjectsFromOld = oldPoint.RestorePoint.ListOfBackupObjects
            .Intersect(newPoint.RestorePoint.ListOfBackupObjects, new BackupObjectComparer())
            .ToList();

        return intersectObjectsFromOld;
    }

    private List<IBackupObject> MergeExceptObjects(RestorePointInfo oldPoint, RestorePointInfo newPoint, IExtraWritingRepository writingRepository)
    {
        if (newPoint.RestorePoint.Storage == null || oldPoint.RestorePoint.Storage == null) return new List<IBackupObject>();

        var exceptObjects = oldPoint.RestorePoint.ListOfBackupObjects
            .Except(newPoint.RestorePoint.ListOfBackupObjects, new BackupObjectComparer())
            .ToList();

        return exceptObjects;
    }
}
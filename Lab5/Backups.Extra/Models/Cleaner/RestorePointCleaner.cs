using Backups.Entities;
using Backups.Extra.Entities;

namespace Backups.Extra.Models.Cleaner;
public class RestorePointCleaner
{
    public RestorePointCleaner(IRestorePointLimit restorePointLimit, IMerger merger)
    {
        RestorePointLimit = restorePointLimit;
        Merger = merger;
    }

    public IRestorePointLimit RestorePointLimit { get; private set; }
    public IMerger Merger { get; private set; }

    public List<IndexedRestorePointInfo> Select(IReadOnlyList<RestorePointInfo> restorePointInfos)
    {
        var listToCheck = new List<IndexedRestorePointInfo>();
        for (int i = 0; i < restorePointInfos.Count; i++)
        {
            listToCheck.Add(new IndexedRestorePointInfo(restorePointInfos[i], restorePointInfos.Count - i));
        }

        return listToCheck.Where(x => RestorePointLimit.SelectToClear(x)).ToList();
    }

    public RestorePointInfo Merge(RestorePointInfo oldPoint, RestorePointInfo newPoint, IExtraWritingRepository writingRepository)
    {
        var restorePoint = new RestorePointInfo(
            new RestorePoint(
                newPoint.RestorePoint.Name,
                newPoint.RestorePoint.Date,
                newPoint.RestorePoint.Version,
                Merger.MergeBackupObjects(oldPoint,  newPoint, writingRepository)),
            newPoint.StorageAlgorithm);

        restorePoint.RestorePoint.AddStorage(new Storage(Merger.MergeBackupZipArchives(oldPoint,  newPoint, writingRepository)));

        writingRepository.Delete(oldPoint.RestorePoint);

        return restorePoint;
    }
}
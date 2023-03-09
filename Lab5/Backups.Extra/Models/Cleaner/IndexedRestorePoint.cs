using Backups.Extra.Entities;

namespace Backups.Extra.Models.Cleaner;

public class IndexedRestorePointInfo
{
    public IndexedRestorePointInfo(RestorePointInfo restorePointInfo, int index)
    {
        RestorePointInfo = restorePointInfo;
        Index = index;
    }

    public RestorePointInfo RestorePointInfo { get; }
    public int Index { get; }
}
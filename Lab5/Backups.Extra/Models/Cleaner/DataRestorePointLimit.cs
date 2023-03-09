namespace Backups.Extra.Models.Cleaner;

public class DataRestorePointLimit : IRestorePointLimit
{
    public DataRestorePointLimit(TimeSpan timeSpan)
    {
        TimeSpan = timeSpan;
    }

    public TimeSpan TimeSpan { get; }

    public bool SelectToClear(IndexedRestorePointInfo p)
    {
        return p.RestorePointInfo.RestorePoint.Date - DateTime.Now > TimeSpan;
    }
}
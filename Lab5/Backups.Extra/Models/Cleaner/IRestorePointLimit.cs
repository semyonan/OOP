namespace Backups.Extra.Models.Cleaner;

public interface IRestorePointLimit
{
    public bool SelectToClear(IndexedRestorePointInfo p);
}
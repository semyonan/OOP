namespace Backups.Extra.Models.Cleaner;

public class AmountRestorePointLimit : IRestorePointLimit
{
    public AmountRestorePointLimit(int limit)
    {
        if (limit <= 0)
        {
            throw new Exception();
        }

        Limit = limit;
    }

    public int Limit { get; }

    public bool SelectToClear(IndexedRestorePointInfo p)
    {
        return p.Index > Limit;
    }
}
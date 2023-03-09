namespace Backups.Extra.Models.Cleaner;

public class HybridRestorePointLimit : IRestorePointLimit
{
    private readonly List<IRestorePointLimit> _listOfRestorePointLimits;
    public HybridRestorePointLimit(List<IRestorePointLimit> limits, bool onlyAll)
    {
        _listOfRestorePointLimits = new List<IRestorePointLimit>(limits);
        OnlyAll = onlyAll;
    }

    public IReadOnlyList<IRestorePointLimit> ListOfRestorePointLimits => _listOfRestorePointLimits.AsReadOnly();
    public bool OnlyAll { get; }

    public bool SelectToClear(IndexedRestorePointInfo p)
    {
        return OnlyAll ? SelectIfAll(p) : SelectIfAny(p);
    }

    private bool SelectIfAll(IndexedRestorePointInfo p)
    {
        return _listOfRestorePointLimits.All(x => x.SelectToClear(p));
    }

    private bool SelectIfAny(IndexedRestorePointInfo p)
    {
        return _listOfRestorePointLimits.Any(x => x.SelectToClear(p));
    }
}
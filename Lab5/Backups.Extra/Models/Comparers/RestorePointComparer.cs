using Backups.Entities;

namespace Backups.Extra.Models.Comparers;

public class RestorePointComparer : IEqualityComparer<RestorePoint>
{
    public bool Equals(RestorePoint? x, RestorePoint? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.Date.Equals(y.Date) && x.Version == y.Version && x.Name == y.Name;
    }

    public int GetHashCode(RestorePoint obj)
    {
        return HashCode.Combine(obj.Date, obj.Version, obj.Name);
    }
}
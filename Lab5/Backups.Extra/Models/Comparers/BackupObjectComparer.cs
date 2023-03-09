using Backups.Entities;

namespace Backups.Extra.Models.Comparers;

public class BackupObjectComparer : IEqualityComparer<IBackupObject>
{
    public bool Equals(IBackupObject? x, IBackupObject? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.FullPathName == y.FullPathName && x.Name == y.Name;
    }

    public int GetHashCode(IBackupObject obj)
    {
        return HashCode.Combine(obj.FullPathName, obj.Name);
    }
}
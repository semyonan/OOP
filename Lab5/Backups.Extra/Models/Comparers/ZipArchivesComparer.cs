using Backups.Models;

namespace Backups.Extra.Models.Comparers;

public class ZipArchivesComparer : IEqualityComparer<BackupZipArchive>
{
    public bool Equals(BackupZipArchive? x, BackupZipArchive? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        return x.GetType() == y.GetType() && new ByteArrayComparer().Equals(x.Data, y.Data);
    }

    public int GetHashCode(BackupZipArchive obj)
    {
        return HashCode.Combine(new ByteArrayComparer().GetHashCode(obj.Data), obj.Name);
    }
}
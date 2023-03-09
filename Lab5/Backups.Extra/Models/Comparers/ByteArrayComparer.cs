namespace Backups.Extra.Models.Comparers;

public class ByteArrayComparer
{
    public bool Equals(byte[] x, byte[] y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x.GetType() != y.GetType()) return false;
        return GetHashCode(x) == GetHashCode(y);
    }

    public int GetHashCode(byte[] data)
    {
        int hash = 17;
        foreach (var element in data)
        {
            hash = (hash * 31) + element.GetHashCode();
        }

        return hash;
    }
}
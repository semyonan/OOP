namespace Backups.Entities;

public interface IReadingRepository
{
    public byte[] GetData(string fullPathName);

    public string[] GetFiles(string fullPathName);

    public string[] GetDirectories(string fullPathName);
}
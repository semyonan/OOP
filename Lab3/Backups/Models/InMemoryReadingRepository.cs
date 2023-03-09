using Backups.Entities;

namespace Backups.Models;

public class InMemoryReadingRepository : IReadingRepository
{
    public byte[] GetData(string fullPathName)
    {
        return Array.Empty<byte>();
    }

    public string[] GetFiles(string fullPathName)
    {
        return Array.Empty<string>();
    }

    public string[] GetDirectories(string fullPathName)
    {
        return Array.Empty<string>();
    }
}
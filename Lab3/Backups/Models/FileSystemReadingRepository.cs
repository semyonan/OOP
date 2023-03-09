using Backups.Entities;
using Backups.Exceptions;

namespace Backups.Models;

public class FileSystemReadingRepository : IReadingRepository
{
    public byte[] GetData(string fullPathName)
    {
        return File.ReadAllBytes(fullPathName);
    }

    public string[] GetFiles(string fullPathName)
    {
        return Directory.GetFiles(fullPathName);
    }

    public string[] GetDirectories(string fullPathName)
    {
        return Directory.GetDirectories(fullPathName);
    }
}
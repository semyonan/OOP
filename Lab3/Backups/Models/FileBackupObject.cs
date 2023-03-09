using Backups.Entities;
using Backups.Exceptions;

namespace Backups.Models;

public class FileBackupObject : IBackupObject
{
    public FileBackupObject(string fullPathName)
    {
        if (string.IsNullOrWhiteSpace(fullPathName))
        {
            throw new BackupsException("Invalid arguments");
        }

        FullPathName = fullPathName;
        Name = Path.GetFileNameWithoutExtension(fullPathName);
        Extension = Path.GetExtension(fullPathName);
    }

    public string Name { get; }
    public string FullPathName { get; }

    public string Extension { get; }
}
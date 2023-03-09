using Backups.Entities;
using Backups.Exceptions;

namespace Backups.Models;

public class FolderBackupObject : IBackupObject
{
    public FolderBackupObject(string fullPathName)
    {
        if (string.IsNullOrWhiteSpace(fullPathName))
        {
            throw new BackupsException("Invalid arguments");
        }

        FullPathName = fullPathName;
        var dir = new DirectoryInfo(fullPathName);
        Name = dir.Name;
    }

    public string FullPathName { get; }
    public string Name { get; }
}
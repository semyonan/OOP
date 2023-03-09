using Backups.Models;

namespace Backups.Entities;

public class Storage
{
    private readonly List<BackupZipArchive> _listOfZipArchives;
    public Storage(List<BackupZipArchive> listOfZipArchives)
    {
        _listOfZipArchives = new List<BackupZipArchive>(listOfZipArchives);
    }

    public IReadOnlyList<BackupZipArchive> ListOfZipArchives => _listOfZipArchives.AsReadOnly();
}
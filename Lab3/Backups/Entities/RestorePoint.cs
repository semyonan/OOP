using System.Net;
using Backups.Exceptions;

namespace Backups.Entities;

public class RestorePoint
{
    private readonly List<IBackupObject> _listOfBackupObjects;
    public RestorePoint(string name, DateTime dateTime, uint version, List<IBackupObject> listOfBackupObjects)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BackupsException("Invalid arguments");
        }

        Name = name;
        _listOfBackupObjects = new List<IBackupObject>(listOfBackupObjects);
        Date = dateTime;
        Version = version;
    }

    public IReadOnlyList<IBackupObject> ListOfBackupObjects => _listOfBackupObjects.AsReadOnly();
    public Storage? Storage { get; private set; }
    public DateTime Date { get; }
    public uint Version { get; }
    public string Name { get; }

    public void AddStorage(Storage storage)
    {
        Storage = storage;
    }

    public override string ToString()
    {
        return Name.ToString();
    }
}
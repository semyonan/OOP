using Backups.Entities;

namespace Backups.Extra.Models.Restore;

public class Unstorage
{
    private readonly List<Tuple<IBackupObject, IUnzipedObject>> _objectsAndItsUnzipedFiles;

    public Unstorage(List<Tuple<IBackupObject, IUnzipedObject>> objectsAndItsUnzipedFiles)
    {
        _objectsAndItsUnzipedFiles = new List<Tuple<IBackupObject, IUnzipedObject>>(objectsAndItsUnzipedFiles);
    }

    public IReadOnlyList<Tuple<IBackupObject, IUnzipedObject>> ListOfObjectsAndItsRezipedFiles => _objectsAndItsUnzipedFiles.AsReadOnly();
}
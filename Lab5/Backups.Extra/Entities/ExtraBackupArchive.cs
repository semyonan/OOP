using Backups.Entities;
using Backups.Models;
namespace Backups.Extra.Entities;

public class ExtraBackupArchive : BackupZipArchive
{
    public ExtraBackupArchive(string name, byte[] data)
        : base(name, new List<IBackupObject>(), new InMemoryReadingRepository())
    {
        Data = data;
    }

    public new byte[] Data { get; }
}
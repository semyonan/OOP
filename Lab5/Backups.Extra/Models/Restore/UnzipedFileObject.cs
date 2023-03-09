using System.IO.Compression;
using Backups.Entities;
using Backups.Extra.Entities;
using Backups.Models;

namespace Backups.Extra.Models.Restore;

public class UnzipedFileObject : IUnzipedObject
{
    public UnzipedFileObject(string name, BackupZipArchive zipArchive)
    {
        var decompressedFileStream = new MemoryStream();
        var compressedFileStream = new MemoryStream(zipArchive.Data);

        using (var zip = new ZipArchive(compressedFileStream, ZipArchiveMode.Read, false))
        {
            foreach (ZipArchiveEntry entry in zip.Entries)
            {
                Console.WriteLine($"{entry.FullName} {entry.Name}");
                var sr = entry.Open();
                sr.CopyTo(decompressedFileStream);
            }
        }

        File = new UnzipedFile(decompressedFileStream.ToArray(), name);
    }

    public UnzipedFileObject(byte[] data, string name)
    {
        File = new UnzipedFile(data, name);
    }

    public UnzipedFile File { get; }

    public void AcceptWriting(IExtraWritingRepository writingRepository, IBackupObject backupObject)
    {
        writingRepository.AddRezipedFile(this, backupObject);
    }
}
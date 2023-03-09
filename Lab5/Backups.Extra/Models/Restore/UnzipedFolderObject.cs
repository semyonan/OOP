using System.IO.Compression;
using Backups.Entities;
using Backups.Extra.Entities;
using Backups.Models;

namespace Backups.Extra.Models.Restore;

public class UnzipedFolderObject : IUnzipedObject
{
    private readonly List<UnzipedFile> _listOfUnzipedFiles;
    private readonly List<string> _listOfUnzipedFolders;

    public UnzipedFolderObject(string name, BackupZipArchive zipArchive)
    {
        Name = name;
        _listOfUnzipedFiles = new List<UnzipedFile>();
        _listOfUnzipedFolders = new List<string>();
        var compressedFileStream = new MemoryStream(zipArchive.Data);

        using (var zip = new ZipArchive(compressedFileStream, ZipArchiveMode.Read, false))
        {
            foreach (ZipArchiveEntry entry in zip.Entries)
            {
                if (IsDirectory(entry))
                {
                    _listOfUnzipedFolders.Add(entry.FullName);
                    break;
                }

                var decompressedFileStream = new MemoryStream();
                var sr = entry.Open();
                sr.CopyTo(decompressedFileStream);
                _listOfUnzipedFiles.Add(new UnzipedFile(decompressedFileStream.ToArray(), entry.FullName));
            }
        }
    }

    public UnzipedFolderObject(List<UnzipedFile> listOfRezipedFiles, List<string> listOfRezipedFolders, string name)
    {
        _listOfUnzipedFiles = new List<UnzipedFile>(listOfRezipedFiles);
        _listOfUnzipedFolders = new List<string>(listOfRezipedFolders);
        Name = name;
    }

    public IReadOnlyList<UnzipedFile> ListOfUnzipedFiles => _listOfUnzipedFiles.AsReadOnly();
    public IReadOnlyList<string> ListOfUnzipedFolders => _listOfUnzipedFolders.AsReadOnly();
    public string Name { get; }

    public void AcceptWriting(IExtraWritingRepository writingRepository, IBackupObject backupObject)
    {
        writingRepository.AddRezipedFolder(this, backupObject);
    }

    private bool IsDirectory(ZipArchiveEntry entry)
    {
        return (entry.FullName.Length > 0) && (entry.FullName[^1] == '/');
    }
}
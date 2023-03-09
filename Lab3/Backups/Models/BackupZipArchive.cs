using System.IO.Compression;
using Backups.Entities;
using Backups.Exceptions;

namespace Backups.Models;

public class BackupZipArchive
{
    private ZipArchive _zipArchive;
    private IReadingRepository _readingRepository;
    public BackupZipArchive(string name, List<IBackupObject> listOfBackupObjects, IReadingRepository readingRepository)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BackupsException("Invalid argument");
        }

        Name = name;
        _readingRepository = readingRepository;

        var compressedFileStream = new MemoryStream();
        using (compressedFileStream)
        {
            using (var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Update, false))
            {
                _zipArchive = zipArchive;
                foreach (var backupObject in listOfBackupObjects)
                {
                    Add(backupObject);
                }
            }
        }

        Data = compressedFileStream.ToArray();
    }

    public string Name { get; }
    public byte[] Data { get; }

    public void Add(IBackupObject backupObject)
    {
        if (backupObject is FileBackupObject file)
        {
            Add(file);
        }

        if (backupObject is FolderBackupObject folder)
        {
            Add(folder);
        }
    }

    public override string ToString()
    {
        return Name.ToString();
    }

    private void Add(FileBackupObject file)
    {
        var zipEntry = _zipArchive.CreateEntry(CreateCopyFileName($"{file.Name}{file.Extension}"));
        var originalFileStream = new MemoryStream(_readingRepository.GetData(file.FullPathName));
        using (var zipEntryStream = zipEntry.Open())
        {
            originalFileStream.CopyTo(zipEntryStream);
        }
    }

    private void Add(FolderBackupObject folder)
    {
        AddDirectory(folder.FullPathName, CreateCopyFolderName(folder.Name));
    }

    private void AddDirectory(string sourceName, string dectinationName)
    {
        var filesPaths = _readingRepository.GetFiles(sourceName);
        var foldersPaths = _readingRepository.GetDirectories(sourceName);

        foreach (var path in filesPaths)
        {
            var zipEntry = _zipArchive.CreateEntry(Path.Combine(dectinationName, Path.GetFileName(path)));
            var originalFileStream = new MemoryStream(_readingRepository.GetData(path));
            using (var zipEntryStream = zipEntry.Open())
            {
                originalFileStream.CopyTo(zipEntryStream);
            }
        }

        foreach (var path in foldersPaths)
        {
            AddDirectory(path, Path.Combine(dectinationName, new DirectoryInfo(path).Name));
        }

        if (filesPaths.Length == 0 && foldersPaths.Length == 0)
        {
            var zipEntry = _zipArchive.CreateEntry($"{dectinationName}/");
        }
    }

    private string CreateCopyFileName(string filename)
    {
        int sameFilesNameCount = 0;
        string copyName = filename;
        while (_zipArchive.Entries.Any(x => x.FullName == copyName))
        {
            sameFilesNameCount++;
            copyName =
                $"{Path.GetFileNameWithoutExtension(filename)}_{sameFilesNameCount}{Path.GetExtension(filename)}";
        }

        return copyName;
    }

    private string CreateCopyFolderName(string foldername)
    {
        int sameFoldersNameCount = 0;
        string copyName = foldername;
        while (_zipArchive.Entries.Any(x => x.FullName == copyName))
        {
            sameFoldersNameCount++;
            copyName =
                $"{Path.GetFileNameWithoutExtension(foldername)}_{sameFoldersNameCount}";
        }

        return copyName;
    }
}
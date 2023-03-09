using Backups.Entities;
using Backups.Models;
using Xunit;

namespace Backups.Test;

public class BackupsTest
{
    [Fact]
    public void CreateSplitStorageBackup()
    {
       var test = new BackupTask(
            "Test",
            new BackupTaskConfiguration(
                new InMemoryReadingRepository(),
                new InMemoryWritingRepository("/Root", new SplitStorageAlgorithm())));

       test.AddBackupObject(new FileBackupObject("FileA.txt"));
       test.AddBackupObject(new FolderBackupObject("FolderB"));
       test.CreateRestorePoint();
       test.RemoveBackupObject(new FolderBackupObject("FolderB"));
       test.CreateRestorePoint();

       Assert.True(test.ListOfRestorePoints.Count == 2);
       Assert.True(test.ListOfRestorePoints[0].Storage?.ListOfZipArchives.Count == 2);
       Assert.True(test.ListOfRestorePoints[1].Storage?.ListOfZipArchives.Count == 1);
    }

    [Fact]
    public void CreateSingleStorageBackup()
    {
        var test = new BackupTask(
            "Test",
            new BackupTaskConfiguration(
                new InMemoryReadingRepository(),
                new InMemoryWritingRepository("/Root", new SingleStorageAlgorithm())));
        test.AddBackupObject(new FileBackupObject("FileA.txt"));
        test.AddBackupObject(new FolderBackupObject("FolderB"));
        test.CreateRestorePoint();
        test.CreateRestorePoint();

        Assert.True(test.ListOfRestorePoints.Count == 2);
        Assert.True(test.ListOfRestorePoints[0].Storage?.ListOfZipArchives.Count == 1);
        Assert.True(test.ListOfRestorePoints[1].Storage?.ListOfZipArchives.Count == 1);
    }
}
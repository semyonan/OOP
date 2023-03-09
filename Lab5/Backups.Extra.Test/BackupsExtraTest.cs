using Backups.Extra.Entities;
using Backups.Extra.Models.Cleaner;
using Backups.Models;
using Xunit;

namespace Backups.Extra.Test;

public class BackupsExtraTest
{
    [Fact]
    public void UnbackupRestorePoint()
    {
        var test = new ExtraBackupTask(
            "Test",
            new InitialInMemoryRepositoryConfiguration(
                new InMemoryReadingRepository(),
                new InMemoryExtraWritingRepository("/Root", new RestoringSplitStorageAlgorithm())),
            new RestorePointCleaner(new AmountRestorePointLimit(3), new Merger()),
            new InMemoryExtraWritingRepository("/Root", new RestoringSplitStorageAlgorithm()));

        test.AddBackupObject(new FileBackupObject("FileA.txt"));
        test.AddBackupObject(new FolderBackupObject("FolderB"));
        test.CreateRestorePoint();
        test.RemoveBackupObject(new FolderBackupObject("FolderB"));
        test.CreateRestorePoint();

        test.UnbackRestorePoint(test.ListOfRestorePointsInfos[0].RestorePoint);

        Assert.True(test.ListOfRestorePointsInfos.Count == 1);
    }

    [Fact]
    public void CleanRestorePoints()
    {
        var test = new ExtraBackupTask(
            "Test",
            new InitialInMemoryRepositoryConfiguration(
                new InMemoryReadingRepository(),
                new InMemoryExtraWritingRepository("/Root", new RestoringSplitStorageAlgorithm())),
            new RestorePointCleaner(new AmountRestorePointLimit(1), new Merger()),
            new InMemoryExtraWritingRepository("/Root", new RestoringSplitStorageAlgorithm()));

        test.AddBackupObject(new FileBackupObject("FileA.txt"));
        test.AddBackupObject(new FolderBackupObject("FolderB"));
        test.CreateRestorePoint();
        test.RemoveBackupObject(new FolderBackupObject("FolderB"));
        test.CreateRestorePoint();
        test.CreateRestorePoint();
        test.CleanRestorePoints();

        Assert.True(test.ListOfRestorePointsInfos.Count == 2);
        Assert.True(test.ListOfRestorePointsInfos[1].RestorePoint.Storage?.ListOfZipArchives.Count == 2);
    }
}
using Backups.Extra.Entities;
using Backups.Extra.Models.Cleaner;
using Backups.Extra.Models.Logger;
using Backups.Models;

namespace Backups.Extra.Console;

class Program
{
    static void Main(string[] args)
    {
        var test = new ExtraBackupTask(
            "Test",
            new InitialFileSystemRepositoryConfiguration(
                new FileSystemReadingRepository(),
                new ExtraFileSystemWritingRepository(@"/Users/anya/Documents/Backup1", new RestoringSplitStorageAlgorithm())),
            new RestorePointCleaner(new AmountRestorePointLimit(3), new Merger()),
            new ExtraFileSystemWritingRepository(@"/Users/anya/Documents/Backup1", new RestoringSplitStorageAlgorithm()),
            new ConsoleLogger(new DateMessageConfigurator()));
        
        test.AddBackupObject(new FileBackupObject(@"/Users/anya/Documents/Test/FileA.pdf"));
        test.AddBackupObject(new FileBackupObject(@"/Users/anya/Documents/FileA.pdf"));
        test.AddBackupObject(new FileBackupObject(@"/Users/anya/Documents/Test/FileB.pdf"));
        test.AddBackupObject(new FileBackupObject(@"/Users/anya/Documents/Test/FileC.pdf"));
        test.AddBackupObject(new FolderBackupObject(@"/Users/anya/Documents/Test/FolderD"));

        test.CreateRestorePoint();
        test.RemoveBackupObject(new FileBackupObject(@"/Users/anya/Documents/Test/FileB.pdf"));
        test.CreateRestorePoint();
        test.CreateRestorePoint();
        test.ChangeStorageAlgorithm(new RestoringSingleStorageAlgorithm());

        test.CreateRestorePoint();
        test.CreateRestorePoint();

        var save = test.Save(@"/Users/anya/Documents/Backup1");
        test.Set(save);
        
        test.CleanRestorePoints();
    }
}
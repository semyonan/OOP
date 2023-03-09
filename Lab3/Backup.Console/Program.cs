using Backups.Entities;
using Backups.Models;

namespace Backup.Console;

class Program
{
    static void Main(string[] args)
    {
        var test = new BackupTask(
            "Test",
            new BackupTaskConfiguration(
                new FileSystemReadingRepository(),
                new FileSystemWritingRepository(@"/Users/anya/Documents/Backup1", new SingleStorageAlgorithm())));
        
        test.AddBackupObject(new FileBackupObject(@"/Users/anya/Documents/Test/FileA.pdf"));
        test.AddBackupObject(new FileBackupObject(@"/Users/anya/Documents/FileA.pdf"));
        test.AddBackupObject(new FileBackupObject(@"/Users/anya/Documents/Test/FileB.pdf"));
        test.AddBackupObject(new FileBackupObject(@"/Users/anya/Documents/Test/FileC.pdf"));
        test.AddBackupObject(new FolderBackupObject(@"/Users/anya/Documents/Test/FolderD"));
        
        test.CreateRestorePoint();
        test.CreateRestorePoint();
        test.ChangeStorageAlgorithm(new SplitStorageAlgorithm());
        test.CreateRestorePoint();
        test.CreateRestorePoint(); 
    }
}
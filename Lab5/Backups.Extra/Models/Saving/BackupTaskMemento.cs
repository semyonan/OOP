using System.Text;
using Backups.Entities;
using Backups.Extra.Entities;
using Backups.Extra.Exceptions;
using Backups.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Backups.Extra.Models.Saving;

public class BackupTaskMemento
{
    public string? ConfigurationPathName { get; private set; }
    public void Serialize(ExtraBackupTask backupTask, string path)
    {
        var content = new BackupTaskContent(backupTask);
        var data = JsonConvert.SerializeObject(content);
        ConfigurationPathName = Path.Combine(path, $"{backupTask.Name}_configuration.json");
        File.WriteAllText(ConfigurationPathName, data);
    }

    public BackupTaskContent? Deserialize()
    {
        if (ConfigurationPathName == null)
        {
            throw new BackupExtraException("Object wasn't serialized");
        }

        var json = File.ReadAllText(ConfigurationPathName);

        var objects = JToken.Parse(json);

        var result = new BackupTaskContent();

        result.VersionCount = uint.Parse(objects["VersionCount"].ToString());

        var objectsList = new List<IBackupObject>();
        foreach (var item in JArray.Parse(objects["ListOfBackupObjectsContents"].ToString()))
        {
            if (item["Type"].ToString() ==
                "Backups.Models.FileBackupObject, Backups, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
            {
                objectsList.Add(new FileBackupObject(item["BackupObject"]["FullPathName"].ToString()));
            }

            if (item["Type"].ToString() ==
                "Backups.Models.FolderBackupObject, Backups, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
            {
                objectsList.Add(new FolderBackupObject(item["BackupObject"]["FullPathName"].ToString()));
            }
        }

        result.AddBackupObjectsList(objectsList);

        var pointsList = new List<RestorePointInfo>();

        foreach (var item in JArray.Parse(objects["ListOfRestorePoints"].ToString()))
        {
            var restorePointObjectsList = new List<IBackupObject>();
            foreach (var obj in JArray.Parse(item["RestorePoint"]["ListOfBackupObjects"].ToString()))
            {
                var tmp = obj;
                if (JObject.Parse(obj.ToString()).TryGetValue("Extension", out tmp))
                {
                    restorePointObjectsList.Add(new FileBackupObject(obj["FullPathName"].ToString()));
                }
                else
                {
                    restorePointObjectsList.Add(new FolderBackupObject(obj["FullPathName"].ToString()));
                }
            }

            var restorePointArchivesList = new List<BackupZipArchive>();
            foreach (var obj in JArray.Parse(item["RestorePoint"]["Storage"]["ListOfZipArchives"].ToString()))
            {
                restorePointArchivesList.Add(new ExtraBackupArchive(
                    obj["Name"].ToString(),
                    Encoding.ASCII.GetBytes(obj["Data"].ToString())));
            }

            var date = item["RestorePoint"]["Date"].ToString();

            var restorePoint = new RestorePoint(
                item["RestorePoint"]["Name"].ToString(),
                DateTime.Parse(date),
                uint.Parse(item["RestorePoint"]["Version"].ToString()),
                objectsList);
            restorePoint.AddStorage(new Storage(restorePointArchivesList));

            pointsList.Add(new RestorePointInfo(
                new RestorePoint(
                    item["RestorePoint"]["Name"].ToString(),
                    DateTime.Parse(date),
                    uint.Parse(item["RestorePoint"]["Version"].ToString()),
                    objectsList), new RestoringSingleStorageAlgorithm()));
        }

        result.AddRestorePointsList(pointsList);

        return result;
    }
}
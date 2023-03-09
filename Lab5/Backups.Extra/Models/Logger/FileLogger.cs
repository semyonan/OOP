namespace Backups.Extra.Models.Logger;

public class FileLogger : ILogger
{
    public FileLogger(string fullPathName, IMessageConfigurator? messageConfigurator = null)
    {
        FullPathName = fullPathName;
        MessageConfigurator = messageConfigurator;
        File = new StreamWriter(fullPathName, append: true);
    }

    public StreamWriter File { get; }
    public string FullPathName { get; }
    public IMessageConfigurator? MessageConfigurator { get; }

    public void Log(string message)
    {
        File.Write(MessageConfigurator == null ? message : MessageConfigurator.Config(message));
    }
}
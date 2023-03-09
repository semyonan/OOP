namespace Backups.Extra.Models.Logger;
public interface ILogger
{
    public IMessageConfigurator? MessageConfigurator { get; }
    public void Log(string message);
}
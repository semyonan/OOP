namespace Backups.Extra.Models.Logger;

public class ConsoleLogger : ILogger
{
    public ConsoleLogger(IMessageConfigurator? messageConfigurator = null)
    {
        MessageConfigurator = messageConfigurator;
    }

    public IMessageConfigurator? MessageConfigurator { get; }

    public void Log(string message)
    {
        Console.WriteLine(MessageConfigurator == null ? message : MessageConfigurator.Config(message));
    }
}
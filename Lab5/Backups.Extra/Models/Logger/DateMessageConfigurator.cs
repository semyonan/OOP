namespace Backups.Extra.Models.Logger;

public class DateMessageConfigurator : IMessageConfigurator
{
    public string Config(string message)
    {
        return $"{DateTime.Now} {message}";
    }
}
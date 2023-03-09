namespace Banks.Models;

public class ConsoleMessageHandler : IMessageHandler
{
    public void Handle(string message)
    {
        Console.WriteLine(message);
    }
}
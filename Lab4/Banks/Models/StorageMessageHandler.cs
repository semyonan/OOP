namespace Banks.Models;

public class StorageMessageHandler : IMessageHandler
{
    private readonly List<string> _messages;

    public StorageMessageHandler()
    {
        _messages = new List<string>();
    }

    public IReadOnlyList<string> Messages => _messages.AsReadOnly();
    public void Handle(string message)
    {
        _messages.Add(message);
    }
}
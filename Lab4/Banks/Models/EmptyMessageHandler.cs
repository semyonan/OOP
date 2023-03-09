namespace Banks.Models;

public class EmptyMessageHandler : IMessageHandler
{
    public void Handle(string message)
    {
    }
}
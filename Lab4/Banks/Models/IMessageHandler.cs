namespace Banks.Models;

public interface IMessageHandler
{
    public void Handle(string message);
}
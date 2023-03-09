namespace DataBaseAccess.Models;
[Serializable]
public class MessageSourceAccount
{
    public MessageSourceAccount(string owner, string login, string password, string type)
    {
        Owner = owner;
        Login = login;
        Password = password;
        Type = type;
    }

    public string Owner { get; }
    public string Login { get; }
    public string Password { get; }
    public string Type { get; }
}
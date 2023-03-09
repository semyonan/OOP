using DataBaseAccess;

namespace Reports.Service.Entities;

public class AuthorizationService : IAuthorizationService
{
    private readonly DataBase _dataBase;
    public AuthorizationService()
    {
        _dataBase = Context.GetInstance().DataBase;
    }

    public bool Authorization(string login, string password, string sourceType)
    {
        if ((password == "password") && (login == "login"))
        {
            Context.GetInstance().SetAccount(null);
            return true;
        }

        var account = _dataBase.Accounts.FirstOrDefault(x => x.Login == login && x.Password == password && x.Type == sourceType);

        if (account == null)
        {
            return false;
        }

        Context.GetInstance().SetAccount(account);
        return true;
    }

    public bool IsAdmin()
    {
        return Context.GetInstance().Account == null;
    }
}
namespace Reports.Service.Entities;

public interface IAuthorizationService
{
    public bool Authorization(string login, string password, string sourceType);
}
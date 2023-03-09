using Reports.Service.Entities;

namespace Reports.Test;

public class Services
{
    private static Services? _instanse = null;

    public Services(
        IAuthorizationService authorizationService,
        IAdministrationService administrationService,
        IMessageService messageService)
    {
        AuthorizationService = authorizationService;
        AdministrationService = administrationService;
        MessageService = messageService;
        _instanse = this;
    }

    public IAuthorizationService AuthorizationService { get; }
    public IAdministrationService AdministrationService { get; }
    public IMessageService MessageService { get; }

    public static Services GetInstance()
    {
        if (_instanse == null)
        {
            throw new Exception();
        }

        return _instanse;
    }
}
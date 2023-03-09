using DataBaseAccess.Models;
using Service.Entities;

namespace Reports.Service.Entities;

public class MessengerAccount : MessageSourceAccount
{
    public MessengerAccount(Employee owner)
        : base(owner.Id, $"{owner.PhoneNumber}", $"{owner.Id}", "Messenger")
    {
    }
}
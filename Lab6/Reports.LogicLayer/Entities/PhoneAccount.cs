using DataBaseAccess.Models;
using Service.Entities;

namespace Reports.Service.Entities;

public class PhoneAccount : MessageSourceAccount
{
    public PhoneAccount(Employee owner)
        : base(owner.Id, $"{owner.PhoneNumber}", $"{owner.Id}", "Phone")
    {
    }
}
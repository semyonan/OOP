using DataBaseAccess.Models;
using Service.Entities;

namespace Reports.Service.Entities;

public class EmailAccount : MessageSourceAccount
{
    public EmailAccount(Employee owner)
        : base(owner.Id, $"{owner.SerName}_{owner.Name}@company.ru", $"{owner.Id}", "Email")
    {
    }

    public EmailAccount()
        : base(" ", " ", " ", "Email")
    {
    }
}
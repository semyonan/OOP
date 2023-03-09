using Backups.Models;
using DataBaseAccess.Models;
using Reports.Service.Entities;
using Service.Entities;
using Xunit;

namespace Reports.Test;

public class ReportsTest
{
    [Fact]
    public void AddEmployee()
    {
        var context = new Context();
        var configurationService = new AdministrationService();

        configurationService.AddEmployee("Anna", "Semyononva");

        var id = Guid.Parse(configurationService.GetEmployees()[0]);

        configurationService.AddEmployee("Max", "Semyonov", id);

        var id2 = Guid.Parse(configurationService.GetEmployees()[1]);

        configurationService.AddEmailAccount(id);
        configurationService.AddEmailAccount(id2);

        Assert.True(configurationService.GetEmployees().Count == 2);
        Assert.True(context.DataBase.Employees[1].HigherSupervisor == id.ToString());
    }

    [Fact]
    public void SendAndReceiveMessage()
    {
        Context context = new Context();
        MessageService messageService = new MessageService();
        AdministrationService configurationService = new AdministrationService();

        configurationService.AddEmployee("Anna", "Semyononva");

        var id = Guid.Parse(configurationService.GetEmployees()[0]);

        configurationService.AddEmployee("Max", "Semyonov", id);

        var id2 = Guid.Parse(configurationService.GetEmployees()[1]);

        configurationService.AddEmailAccount(id);
        configurationService.AddEmailAccount(id2);

        messageService.Send("Hello", "Do something", id2, id, new EmailAccount());

        Assert.True(messageService.Receive(id2).Count == 0);
    }
}
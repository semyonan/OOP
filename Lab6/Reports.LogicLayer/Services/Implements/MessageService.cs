using DataBaseAccess;
using DataBaseAccess.Models;
using Reports.Service.Exceptions;
using Service.Entities;

namespace Reports.Service.Entities;

public class MessageService : IMessageService
{
    private readonly DataBase _dataBase;
    public MessageService()
    {
        _dataBase = Context.GetInstance().DataBase;
    }

    public void Send(string title, string text, Guid receiverId, Guid? senderId = null, MessageSourceAccount? messageSourceAccount = null)
    {
        if (messageSourceAccount == null)
        {
            messageSourceAccount = Context.GetInstance().Account;
        }

        if ((senderId == null) && (messageSourceAccount != null))
        {
            senderId = Guid.Parse(messageSourceAccount.Owner);
        }

        if (messageSourceAccount != null)
        {
            _dataBase.Messages.Add(new Message(
                Guid.NewGuid().ToString(),
                title,
                text,
                DateTime.Now,
                receiverId.ToString(),
                senderId.ToString() ?? throw new ReportLogicException("Id is null"),
                messageSourceAccount,
                MessageState.New));
        }
    }

    public List<string> Receive(Guid? receiverId = null)
    {
        string? id = null;

        if ((receiverId == null) && (Context.GetInstance().Account != null))
        {
           id = Context.GetInstance().Account?.Owner;
        }
        else
        {
            id = receiverId.ToString();
        }

        var messages = _dataBase.Messages
            .Where(x =>
            {
                MessageSourceAccount? messageSourceAccount = Context.GetInstance().Account;
                return messageSourceAccount != null
                       && x.ToWhom == id
                       && x.Source.GetType() == messageSourceAccount.GetType();
            }).ToList();
        var indexed = new List<string>();
        for (var i = 0; i < messages.Count; i++)
        {
            _dataBase.ChangeState(messages[i].Id.ToString(), MessageState.Send);
            indexed.Add($"{i} {messages[i].Title}");
        }

        Context.GetInstance().AddMessages(messages);

        return indexed;
    }

    public Tuple<string, string, string> GetWholeMessage(int index)
    {
        var message = Context.GetInstance().Messages[index];

        _dataBase.ChangeState(message.Id, MessageState.Send);

        return new Tuple<string, string, string>($"{message.From}", $"{message.Title}", $"{message.Text}");
    }

    public List<string> GetEmployees()
    {
        var employees = _dataBase.Employees
            .Where(x => x.HigherSupervisor == Context.GetInstance().Account?.Owner)
            .Select(x => x.Id);

        var accounts = _dataBase.Accounts.Where(x =>
            employees.Contains(x.Owner) && x.GetType() == Context.GetInstance().Account?.GetType()).ToList();

        var indexed = new List<string>();

        for (var i = 0; i < accounts.Count; i++)
        {
            indexed.Add($"{i} {accounts[i].Login}");
        }

        Context.GetInstance().AddAccounts(accounts);

        return indexed;
    }

    public string GetWholeAccount(int index)
    {
        var account = Context.GetInstance().Accounts[index];

        return $"{account.Owner}";
    }

    public void CreateReport(
        List<string> commands,
        List<string> messengerTypes,
        DateTime? start = null,
        DateTime? endOf = null,
        Guid? subordinateId = null,
        Guid? id = null)
    {
        string? stringId = null;

        if (id == null)
        {
            stringId = Context.GetInstance().Account?.Owner;
        }
        else
        {
            stringId = id.ToString();
        }

        var builder = new ReportBuilder(_dataBase.Messages
            .Where(x => x.DateTime.Date == DateTime.Today && x.From == stringId).ToList());

        if (commands.Contains("Whole number"))
        {
            builder.AddNumberOfMessages();
        }

        if (commands.Contains("Number by Source"))
        {
            foreach (var type in messengerTypes)
            {
                builder.AddNumberOfMessagesByMessenger(type);
            }
        }

        if (commands.Contains("Number by Time"))
        {
            if ((start != null) && (endOf != null))
            {
                builder.AddNumberOfMessagesByTime(start.Value, endOf.Value);
            }
        }

        if (commands.Contains("Number by Employee"))
        {
            if (id != null)
            {
                builder.AddNumberOfMessagesByEmployee(id.Value.ToString());
            }
        }

        _dataBase.Reports.Add(builder.Build());
    }

    public List<string> GetReports(string? supervisorId = null)
    {
        if (supervisorId == null)
        {
            supervisorId = Context.GetInstance().Account?.Owner;
        }

        var reports = _dataBase.Reports.Where(x => x.Owner == supervisorId).ToList();

        Context.GetInstance().AddReports(reports);

        var indexed = new List<string>();

        for (var i = 0; i < reports.Count; i++)
        {
            indexed.Add($"{i} {reports[i].DateTime}");
        }

        return indexed;
    }

    public string GetWholeReport(int index)
    {
        var report = Context.GetInstance().Reports[index];

        return report.Text;
    }
}
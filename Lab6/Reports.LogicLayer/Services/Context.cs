using DataBaseAccess;
using DataBaseAccess.Models;
using Reports.Service.Exceptions;
using Service.Entities;

namespace Reports.Service.Entities;

public class Context
{
    private static Context? _instance = null;
    private List<Message> _messages;
    private List<MessageSourceAccount> _accounts;
    private List<Report> _reports;
    public Context()
    {
        _instance = this;
        DataBase = DataBase.Deserialize();
        _messages = new List<Message>();
        _accounts = new List<MessageSourceAccount>();
        _reports = new List<Report>();
    }

    public MessageSourceAccount? Account { get; private set; }
    public DataBase DataBase { get; }
    public IReadOnlyList<Message> Messages => _messages.AsReadOnly();
    public IReadOnlyList<MessageSourceAccount> Accounts => _accounts.AsReadOnly();
    public IReadOnlyList<Report> Reports => _reports.AsReadOnly();

    public static Context GetInstance()
    {
        if (_instance == null)
        {
            throw new ReportLogicException("Context must be created");
        }

        return _instance;
    }

    public void AddMessages(List<Message> messages)
    {
        _messages = new List<Message>(messages);
    }

    public void AddAccounts(List<MessageSourceAccount> accounts)
    {
        _accounts = new List<MessageSourceAccount>(accounts);
    }

    public void AddReports(List<Report> reports)
    {
        _reports = new List<Report>(reports);
    }

    public void SetAccount(MessageSourceAccount? account)
    {
        Account = account;
    }
}
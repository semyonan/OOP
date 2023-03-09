using Reports.Service.Entities;
using Reports.Service.Exceptions;

namespace Service.Entities;

public class ReportBuilder
{
    private List<Message> _messageInfos;

    public ReportBuilder(List<Message> messages)
    {
        _messageInfos = new List<Message>(messages);
    }

    public string? Text { get; private set; }

    public ReportBuilder AddNumberOfMessages()
    {
        Text = $"{Text}\nWhole Number {_messageInfos.Count}";
        return this;
    }

    public ReportBuilder AddNumberOfMessagesByMessenger(string source)
    {
        Text = $"{Text}\nMessages by {source} {_messageInfos.Count(x => x.Source.Type == source)}";
        return this;
    }

    public ReportBuilder AddNumberOfMessagesByTime(DateTime start, DateTime end)
    {
        Text = $"{Text}\nMessages between {start} and {end} {_messageInfos.Count(x => x.DateTime <= end && x.DateTime >= start)}";
        return this;
    }

    public ReportBuilder AddNumberOfMessagesByEmployee(string id)
    {
        Text = $"{Text}\nMessages to {id} {_messageInfos.Count(x => x.ToWhom == id)}";
        return this;
    }

    public Report Build()
    {
        if (Text == null)
        {
            throw new ReportLogicException("Text can not be empty");
        }

        return new Report(Text, Context.GetInstance().Account!.Password, DateTime.Now);
    }
}
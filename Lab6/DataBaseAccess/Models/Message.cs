using DataBaseAccess.Models;
using Reports.Service.Entities;

namespace Service.Entities;

public class Message
{
    public Message(string id, string title, string text, DateTime dateTime, string toWhom, string from, MessageSourceAccount source, MessageState state)
    {
        Id = id;
        Title = title;
        Text = text;
        State = state;
        DateTime = dateTime;
        ToWhom = toWhom;
        From = from;
        Source = source;
    }

    public string Id { get; }
    public string Title { get; }
    public string Text { get; }
    public MessageState State { get; set; }
    public MessageSourceAccount Source { get; }
    public DateTime DateTime { get; }
    public string ToWhom { get; }
    public string From { get; }
}
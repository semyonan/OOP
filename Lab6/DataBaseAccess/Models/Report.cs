namespace Service.Entities;

public class Report
{
    public Report(string text, string owner, DateTime dateTime)
    {
        Text = text;
        Owner = owner;
        DateTime = dateTime;
    }

    public string Text { get; }
    public string Owner { get; }
    public DateTime DateTime { get; }
}
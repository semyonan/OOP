using Reports.Service.Entities;
using Spectre.Console;

namespace Reports.Test;

public class SendMessage
{
    public void Run()
    {
        var service = Services.GetInstance().MessageService;
        var account = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[fuchsia]Choose Employee[/]?")
                .PageSize(10)
                .AddChoices(service.GetEmployees()));

        var employee = service.GetWholeAccount(int.Parse(account.Split(' ')[0]));

        var title = AnsiConsole.Ask<string>("[fuchsia]Title:[/]");
        var text = AnsiConsole.Prompt(
            new TextPrompt<string>("[fuchsia]Text:[/]"));

        service.Send(title, text, Guid.Parse(employee));
    }
}
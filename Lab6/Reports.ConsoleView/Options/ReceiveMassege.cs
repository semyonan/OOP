using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Spectre.Console;

namespace Reports.Test;

public class ReceiveMessage
{
    public void Run()
    {
        var service = Services.GetInstance().MessageService;
        var title = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[fuchsia]Choose Messages[/]?")
                .PageSize(10)
                .AddChoices(service.Receive()));

        var message = service.GetWholeMessage(int.Parse(title.Split(' ')[0]));

        var table = new Table();

        table.AddColumn("[fuchsia]From[/]");
        table.AddColumn(message.Item1);

        table.AddRow("[fuchsia]Title[/]", message.Item2);
        table.AddRow("[fuchsia]Text[/]", message.Item3);

        AnsiConsole.Write(table);

        var option = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .PageSize(10)
                .AddChoices(new[]
                {
                    "Exit",
                }));
    }
}
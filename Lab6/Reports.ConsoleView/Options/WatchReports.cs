using System.ComponentModel.Design;
using System.Resources;
using Spectre.Console;

namespace Reports.Test;

public class WatchReports
{
    public void Run()
    {
        var service = Services.GetInstance().MessageService;

        var date = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[fuchsia]Choose report[/]?")
                .PageSize(10)
                .AddChoices(service.GetReports()));

        var report = service.GetWholeReport(int.Parse(date.Split()[0]));

        AnsiConsole.WriteLine(report);
    }
}
using Spectre.Console;

namespace Reports.Test;

public class AddEmployee
{
    public void Run()
    {
        var service = Services.GetInstance().AdministrationService;
        var name = AnsiConsole.Ask<string>("[fuchsia]Name[/]");
        var serName = AnsiConsole.Ask<string>("[fuchsia]Ser name[/]");
        var id = AnsiConsole.Prompt(
            new TextPrompt<string?>($"[fuchsia]Supervisor id[/]")
                .PromptStyle("blue")
                .AllowEmpty());

        if (string.IsNullOrEmpty(id))
        {
            service.AddEmployee(name, serName);
        }
        else
        {
            service.AddEmployee(name, serName, Guid.Parse(id));
        }
    }
}
using Spectre.Console;

namespace Reports.Test;

public class Authorization
{
    public void Run(string? source)
    {
        var service = Services.GetInstance().AuthorizationService;
        var login = AnsiConsole.Ask<string>("[fuchsia]Login:[/]");
        var password = AnsiConsole.Prompt(
            new TextPrompt<string>("[fuchsia]Password[/]")
                .PromptStyle("red")
                .Secret());

        while (source != null && !service.Authorization(login, password, source))
        {
            login = AnsiConsole.Ask<string>("[fuchsia]Login:[/]");
            password = AnsiConsole.Prompt(
                new TextPrompt<string>("[fuchsia]Password[/]")
                    .PromptStyle("red")
                    .Secret());
        }
    }
}
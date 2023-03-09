using Spectre.Console;

namespace Reports.Test;

public class AddSource
{
    public void Run()
    {
        var service = Services.GetInstance().AdministrationService;
        var id = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[fuchsia]Choose employee[/]?")
                .PageSize(10)
                .AddChoices(service.GetEmployees()));

        var source = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[fuchsia]Choose Source[/]")
                .PageSize(10)
                .AddChoices(new[]
                {
                    "Phone",
                    "Email",
                    "Messenger",
                }));

        switch (source)
        {
            case "Phone":
            {
                service.AddPhoneAccount(Guid.Parse(id));
                break;
            }

            case "Email":
            {
                service.AddEmailAccount(Guid.Parse(id));
                break;
            }

            case "Messenger":
            {
                service.AddMessengerAccount(Guid.Parse(id));
                break;
            }
        }
    }
}
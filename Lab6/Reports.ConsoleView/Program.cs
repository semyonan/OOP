using Microsoft.Extensions.DependencyInjection;
using Reports.Service.Entities;
using Reports.Test;
using Spectre.Console;

namespace Reports.ConsoleView;

internal static class Program
{
    private static void Main(string[] args)
    {
        new Context();
        new Services(new AuthorizationService(), new AdministrationService(), new MessageService());

        string option = " ";

        while (option != "Exit")
        {
            option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                        "As admin",
                        "As employee",
                        "Exit",
                    }));

            switch (option)
            {
                case "As admin":
                {
                    new Authorization().Run(null);
                    new AdminView().Run();
                    break;
                }

                case "As employee":
                {
                    var source = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("[fuchsia]Choose Source[/]")
                            .PageSize(10)
                            .AddChoices(new[]
                            {
                                "Phone",
                                "Email",
                                "Messenger",
                                "Exit",
                            }));

                    if (source != "Exit")
                    {
                        new Authorization().Run(source);
                        new SupervisorView().Run();
                    }

                    break;
                }
            }
        }

        Services.GetInstance().AdministrationService.Save();
    }
}
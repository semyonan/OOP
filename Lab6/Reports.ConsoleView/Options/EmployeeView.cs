using Spectre.Console;

namespace Reports.Test;

public class SupervisorView
{
    public void Run()
    {
        var option = " ";

        while (option != "Exit")
        {
            option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[fuchsia]What do you want to do[/]?")
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                        "Send message",
                        "Receive message",
                        "Create report",
                        "Watch reports",
                        "Exit",
                    }));

            switch (option)
            {
                case "Send message":
                {
                    new SendMessage().Run();
                    break;
                }

                case "Create report":
                {
                    new CreateReport().Run();
                    break;
                }

                case "Receive message":
                {
                    new ReceiveMessage().Run();
                    break;
                }

                case "Watch reports":
                {
                    new WatchReports().Run();
                    break;
                }
            }
        }
    }
}
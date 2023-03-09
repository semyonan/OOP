using Spectre.Console;

namespace Reports.Test;

public class AdminView
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
                        "Add employee",
                        "Add source",
                        "Exit",
                    }));

            switch (option)
            {
                case "Add employee":
                {
                    new AddEmployee().Run();
                    break;
                }

                case "Add source":
                {
                    new AddSource().Run();
                    break;
                }
            }
        }
    }
}
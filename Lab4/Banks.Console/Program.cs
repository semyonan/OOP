using Banks.ConsoleOptions;
using Banks.ConsoleOptions.BankOptions;
using Banks.ConsoleOptions.ClientOptions;
using Banks.Entities;
using Spectre.Console;

namespace Banks.Console;

internal static class Program
{
    private static void Main(string[] args)
    {
        string option = " ";

        var centralBank = new CentralBank(new RewindTimeMachine());

        while (option != "Exit")
        {
            option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[fuchsia]What do you want to do[/]?")
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                        "Create Bank", "Create Client", "Create Bank Account",
                        "Change Bank", "Change Client",
                        "Rewind Time",
                        "Make Transaction", "Cancel Transaction",
                        "Exit",
                    }));

            switch (option)
            {
                case "Create Bank":
                {
                    new CreateBank().Run();
                    break;
                }

                case "Create Client":
                {
                    new CreateClient().Run();
                    break;
                }

                case "Create Bank Account":
                {
                    new CreateBankAccount().Run();
                    break;
                }

                case "Change Bank":
                {
                    new ChangeBank().Run();
                    break;
                }

                case "Change Client":
                {
                    new ChangeClient().Run();
                    break;
                }

                case "Rewind Time":
                {
                    new RewindTime().Run();
                    break;
                }

                case "Make Transaction":
                {
                    new MakeTransaction().Run();
                    break;
                }

                case "Cancel Transaction":
                {
                    new CancelTransaction().Run();
                    break;
                }
            }
        }
    }
}
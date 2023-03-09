using Banks.Entities;
using Spectre.Console;

namespace Banks.ConsoleOptions;

public class CreateBankAccount
{
    public void Run()
    {
        var centralBank = CentralBank.GetInstance();

        var listOfClients = centralBank.ListOfClients.Select(x => $"[fuchsia]{x.Name} {x.SerName}[/] {x.Id}")
            .ToList();

        if (!listOfClients.Any())
        {
            AnsiConsole.WriteLine("There is no client to create an account");
            return;
        }

        var clientAsString = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("In which [fuchsia]bank[/] do you want to create account?")
                .PageSize(10)
                .AddChoices(listOfClients));

        var listOfBanks = centralBank.ListOfBanks.Select(x => x.Name).ToList();

        if (!listOfBanks.Any())
        {
            AnsiConsole.WriteLine("There is no bank to create an account");
            return;
        }

        var bankAsString = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("In which [fuchsia]bank[/] do you want to create account?")
                .PageSize(10)
                .AddChoices(listOfBanks));

        var type = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("In which [fuchsia]bank[/] do you want to create account?")
                .PageSize(10)
                .AddChoices(new[]
                {
                    "Debit",
                    "Credit",
                    "Deposit",
                }));

        var subs = clientAsString.Split(' ');
        var clientId = new Guid(subs[^1]);

        switch (type)
        {
            case "Debit":
            {
                centralBank.AddDebitAccount(clientId, bankAsString);
                break;
            }

            case "Deposit":
            {
                int days = AnsiConsole.Prompt(
                    new TextPrompt<int>($"How many days from [fuchsia]{centralBank.TimeMachine.CurDateTime}[/]?")
                        .PromptStyle("green")
                        .Validate(days =>
                        {
                            return days switch
                            {
                                <= 0 => ValidationResult.Error("[red]Days number must be more than a 0[/]"),
                                _ => ValidationResult.Success(),
                            };
                        }));

                centralBank.AddDepositAccount(clientId, bankAsString, new TimeSpan(days, 0, 0, 0));
                break;
            }

            case "Credit":
            {
                centralBank.AddCreditAccount(clientId, bankAsString);
                break;
            }
        }
    }
}
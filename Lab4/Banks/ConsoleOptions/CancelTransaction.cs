using Banks.Entities;
using Spectre.Console;

namespace Banks.ConsoleOptions;

public class CancelTransaction
{
    public void Run()
    {
        var centralBank = CentralBank.GetInstance();
        var listOfTransactions = centralBank.ListOfTransaction
            .Select(x => $"{x.Id}").ToList();

        if (!listOfTransactions.Any())
        {
            AnsiConsole.WriteLine("There is no transaction to cancel");

            return;
        }

        var transactionId = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[fuchsia]Choose transaction to cancel[/]?")
                .PageSize(10)
                .AddChoices(listOfTransactions));

        centralBank.CancelTransaction(new Guid(transactionId));
    }
}
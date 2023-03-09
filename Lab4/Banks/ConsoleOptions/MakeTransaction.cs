using Banks.Entities;
using Banks.Models;
using Spectre.Console;

namespace Banks.ConsoleOptions;

public class MakeTransaction
{
    public void Run()
    {
        var centralBank = CentralBank.GetInstance();

        var listOfTransaction = centralBank.ListOfBanks.SelectMany(x => x.ListOfBankAccounts)
            .Where(x => x is DebitAccount)
            .Select(x => $"{x.Id} [fuchsia]Type:[/]Debit [fuchsia]Owner:[/]{x.Owner.Name}").ToList();

        listOfTransaction.AddRange(centralBank.ListOfBanks.SelectMany(x => x.ListOfBankAccounts)
            .Where(x => x is DepositAccount)
            .Select(x => $"{x.Id} [fuchsia]Type:[/]Deposit [fuchsia]Owner:[/]{x.Owner.Name}").ToList());

        listOfTransaction.AddRange(centralBank.ListOfBanks.SelectMany(x => x.ListOfBankAccounts)
            .Where(x => x is CreditAccount)
            .Select(x => $"{x.Id} [fuchsia]Type:[/]Credit [fuchsia]Owner:[/]{x.Owner.Name}").ToList());

        if (!listOfTransaction.Any())
        {
            AnsiConsole.WriteLine("There is no bank account to create transaction");
            return;
        }

        var accountId = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[fuchsia]Choose bank account[/]?")
                .PageSize(10)
                .AddChoices(listOfTransaction));

        var transaction = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[fuchsia]Change or exit[/]?")
                .PageSize(10)
                .AddChoices(new[]
                {
                    "Withdraw",
                    "Replenishment",
                    "Remittance",
                }));

        int sum = AnsiConsole.Prompt(
            new TextPrompt<int>($"Which [fuchsia]sum[/] do you want to {transaction}?")
                .PromptStyle("green")
                .Validate(initialDepositInterestsLimit =>
                {
                    return initialDepositInterestsLimit switch
                    {
                        <= 0 => ValidationResult.Error("[red]Sum must be more than a 0[/]"),
                        _ => ValidationResult.Success(),
                    };
                }));

        var id = new Guid(accountId.Split(' ')[0]);

        switch (transaction)
        {
            case "Withdraw":
            {
                centralBank.WithdrawTransaction(id, new Money(sum));
                break;
            }

            case "Replenishment":
            {
                centralBank.ReplenishmentTransaction(id, new Money(sum));
                break;
            }

            case "Remittance":
            {
                var listOfReceiverAccounts = centralBank.ListOfBanks.SelectMany(x => x.ListOfBankAccounts)
                    .Where(x => x.Id != id)
                    .Select(x => $"{x.Id} Owner:{x.Owner.Name}").ToList();

                if (!listOfReceiverAccounts.Any())
                {
                    AnsiConsole.WriteLine("There is no account to receive");
                    return;
                }

                var receiverId = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[fuchsia]Choose bank account[/]?")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                        .AddChoices());

                var idRes = new Guid(receiverId.Split(' ')[0]);

                centralBank.RemittanceTransaction(id, idRes, new Money(sum));
                break;
            }
        }
    }
}
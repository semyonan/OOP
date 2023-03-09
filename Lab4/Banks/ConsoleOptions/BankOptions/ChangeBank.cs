using Banks.Entities;
using Spectre.Console;

namespace Banks.ConsoleOptions.BankOptions;

public class ChangeBank
{
    public void Run()
    {
        var option = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[fuchsia]Change or exit[/]?")
                .PageSize(10)
                .AddChoices(new[]
                {
                    "Change",
                    "Exit",
                }));

        var centralBank = CentralBank.GetInstance();

        var listOfBanks = centralBank.ListOfBanks.Select(x => x.Name).ToList();

        if (!listOfBanks.Any())
        {
            AnsiConsole.WriteLine("There is no bank to change");
            return;
        }

        while (option != "Exit")
        {
            var bankName = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[fuchsia]Which bank do you want to change[/]?")
                    .PageSize(10)
                    .AddChoices(listOfBanks));

            var change = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[fuchsia]What do you want to change[/]?")
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                        "Deposit interests",
                        "Credit commission",
                        "Debit Interests",
                        "Withdrawal limit for invalid accounts",
                        "Another bank remittance commission",
                        "Commission for all withdrawals",
                    }));

            var bank = centralBank.GetBank(bankName);

            switch (change)
            {
                case "Deposit interests":
                {
                    centralBank.GetBank(bank.Name).ChangeDepositAccountInfo(new GetDepositAccountInfo().Run());
                    break;
                }

                case "Credit commission":
                {
                    var creditAccountInfo = new GetCreditAccountInfo().Run();
                    centralBank.GetBank(bank.Name).ChangeCreditAccountInfo(creditAccountInfo.Item1, creditAccountInfo.Item2);
                    break;
                }

                case "Debit Interests":
                {
                    centralBank.GetBank(bank.Name).ChangeDebitAccountInfo(new GetDebitAccountInfo().Run());
                    break;
                }

                case "Withdrawal limit for invalid accounts":
                {
                    centralBank.GetBank(bank.Name).ChangeLimitForPrecariousTransactions(new GetWithdrawalLimit().Run());
                    break;
                }

                case "Another bank remittance commission":
                {
                    centralBank.GetBank(bank.Name).ChangeRemittanceCommission(new GetAnotherBankRemittanceCommission().Run());
                    break;
                }

                case "Commission for all withdrawals":
                {
                    var commission = new GetCommissionForAllWithdrawals().Run();
                    centralBank.GetBank(bank.Name).ChangeWithdrawalCommission(commission.Item1, commission.Item2);
                    break;
                }
            }

            option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[fuchsia]Which bank do you want to change[/]?")
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                        "Change",
                        "Exit",
                    }));
        }
    }
}
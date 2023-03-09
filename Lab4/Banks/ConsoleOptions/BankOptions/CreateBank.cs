using Banks.Entities;
using Banks.Models;
using Spectre.Console;

namespace Banks.ConsoleOptions.BankOptions;

public class CreateBank
{
    public void Run()
    {
        var centralBank = CentralBank.GetInstance();
        var builder = new BankConfigurationBuilder();

        var name = AnsiConsole.Ask<string>("What is [fuchsia]bank's name[/]?");

        var creditAccountInfo = new GetCreditAccountInfo().Run();

        builder.CreditAccountInfo(new CreditAccountInfo(creditAccountInfo.Item1, creditAccountInfo.Item2))
            .DebitAccountInfo(new DebitAccountInfo(new GetDebitAccountInfo().Run()))
            .DepositAccountInfo(new DepositAccountInfo(new GetDepositAccountInfo().Run()))
            .LimitForPrecariousTransactions(new GetWithdrawalLimit().Run());

        if (AnsiConsole.Confirm("Do you want to add [fuchsia]commission[/] for remittance to another banks?"))
        {
            builder.AnotherBankRemittanceCommission(new GetAnotherBankRemittanceCommission().Run());
        }

        if (AnsiConsole.Confirm("Do you want to add [fuchsia]commission[/] for all withdrawals?"))
        {
            var commission = new GetCommissionForAllWithdrawals().Run();
            builder.WithdrawalCommission(commission.Item1, commission.Item2);
        }

        centralBank.AddBank(
            name,
            builder.Build());
    }
}
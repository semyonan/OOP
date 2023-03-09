using Banks.Models;
using Spectre.Console;

namespace Banks.ConsoleOptions.BankOptions;

public class GetCommissionForAllWithdrawals
{
    public Tuple<Percent, Money> Run()
    {
        var commissionAllWithdrawal = AnsiConsole.Prompt(
            new TextPrompt<int>($"What is bank [fuchsia]commission[/] for all withdrawals? [fuchsia](%)[/]")
                .PromptStyle("blue")
                .Validate(commissionAllWithdrawal =>
                {
                    return commissionAllWithdrawal switch
                    {
                        <= 0 => ValidationResult.Error("[red]Commission must be more than a 0[/]"),
                        >= 100 => ValidationResult.Error("[red]Commission must be less than 100[/]"),
                        _ => ValidationResult.Success(),
                    };
                }));

        var limitAllWithdrawal = AnsiConsole.Prompt(
            new TextPrompt<int>($"What is bank [fuchsia]limit[/] for all withdrawal without commission?")
                .PromptStyle("blue")
                .Validate(limitAllWithdrawal =>
                {
                    return limitAllWithdrawal switch
                    {
                        <= 0 => ValidationResult.Error("[red]Limit must be more than a 0[/]"),
                        _ => ValidationResult.Success(),
                    };
                }));

        return new Tuple<Percent, Money>(new Percent(commissionAllWithdrawal), new Money(limitAllWithdrawal));
    }
}
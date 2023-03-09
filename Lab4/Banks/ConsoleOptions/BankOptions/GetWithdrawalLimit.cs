using Banks.Models;
using Spectre.Console;

namespace Banks.ConsoleOptions.BankOptions;

public class GetWithdrawalLimit
{
    public Money Run()
    {
        var limitWithdrawal = AnsiConsole.Prompt(
            new TextPrompt<int>($"What is bank [fuchsia]limit[/] for withdrawal for invalid accounts?")
                .PromptStyle("blue")
                .Validate(limitWithdrawal =>
                {
                    return limitWithdrawal switch
                    {
                        <= 0 => ValidationResult.Error("[red]Limit must be more than a 0[/]"),
                        _ => ValidationResult.Success(),
                    };
                }));

        return new Money(limitWithdrawal);
    }
}
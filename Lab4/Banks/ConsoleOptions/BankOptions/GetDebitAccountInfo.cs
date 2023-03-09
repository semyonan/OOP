using Banks.Models;
using Spectre.Console;

namespace Banks.ConsoleOptions.BankOptions;

public class GetDebitAccountInfo
{
    public Percent Run()
    {
        var debitInterests = AnsiConsole.Prompt(
            new TextPrompt<double>($"What is bank [fuchsia]interests[/] per year for debit accounts? [fuchsia](%)[/]")
                .PromptStyle("blue")
                .AllowEmpty()
                .Validate(debitInterests =>
                {
                    return debitInterests switch
                    {
                        <= 0 => ValidationResult.Error("[red]Interests must be more than a 0[/]"),
                        >= 100 => ValidationResult.Error("[red]Interests must be less than 100[/]"),
                        _ => ValidationResult.Success(),
                    };
                }));

        return new Percent(debitInterests);
    }
}
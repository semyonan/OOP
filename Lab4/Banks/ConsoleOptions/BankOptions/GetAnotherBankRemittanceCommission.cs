using Banks.Models;
using Spectre.Console;

namespace Banks.ConsoleOptions.BankOptions;

public class GetAnotherBankRemittanceCommission
{
    public Percent Run()
    {
        var commissionRemittance = AnsiConsole.Prompt(
            new TextPrompt<int>($"What is bank [fuchsia]commission[/] for remittance to another banks? [fuchsia](%)[/]")
                .PromptStyle("blue")
                .Validate(commissionRemittance =>
                {
                    return commissionRemittance switch
                    {
                        <= 0 => ValidationResult.Error("[red]Commission must be more than a 0[/]"),
                        >= 100 => ValidationResult.Error("[red]Commission must be less than 100[/]"),
                        _ => ValidationResult.Success(),
                    };
                }));

        return new Percent(commissionRemittance);
    }
}
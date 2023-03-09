using Banks.Models;
using Spectre.Console;

namespace Banks.ConsoleOptions.BankOptions;

public class GetCreditAccountInfo
{
    public Tuple<Percent, Money> Run()
    {
        var commission = AnsiConsole.Prompt(
            new TextPrompt<double>($"What is bank [fuchsia]commission[/] for credit accounts? [fuchsia](%)[/]")
                .PromptStyle("blue")
                .Validate(commission =>
                {
                    return commission switch
                    {
                        <= 0 => ValidationResult.Error("[red]Commission must be more than a 0[/]"),
                        >= 100 => ValidationResult.Error("[red]Commission must be less than 100[/]"),
                        _ => ValidationResult.Success(),
                    };
                }));

        var limit = AnsiConsole.Prompt(
            new TextPrompt<decimal>($"What is bank [fuchsia]limit[/] for credit accounts?")
                .PromptStyle("blue")
                .Validate(limit =>
                {
                    return limit switch
                    {
                        <= 0 => ValidationResult.Error("[red]Limit ion must be more than a 0[/]"),
                        _ => ValidationResult.Success(),
                    };
                }));

        return new Tuple<Percent, Money>(new Percent(commission), new Money(limit));
    }
}
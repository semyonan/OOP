using Banks.Models;
using Spectre.Console;

namespace Banks.ConsoleOptions.BankOptions;

public class GetDepositAccountInfo
{
    public List<Tuple<Money, Percent>> Run()
    {
        var depositInterests = new List<Tuple<Money, Percent>>();

        int minDepositInterestsPercent = AnsiConsole.Prompt(
            new TextPrompt<int>($"What is bank minimal [fuchsia]interests[/] per year for deposit accounts? [fuchsia](%)[/]")
                .PromptStyle("blue")
                .Validate(initialDepositInterestsPercent =>
                {
                    return initialDepositInterestsPercent switch
                    {
                        <= 0 => ValidationResult.Error("[red]Interests must be more than a 0[/]"),
                        >= 100 => ValidationResult.Error("[red]Interests must be less than 100[/]"),
                        _ => ValidationResult.Success(),
                    };
                }));

        depositInterests.Add(new Tuple<Money, Percent>(new Money(0), new Percent(minDepositInterestsPercent)));

        while (AnsiConsole.Confirm("Add one more interests for deposits accounts?"))
        {
            int depositInterestsPercent = AnsiConsole.Prompt(
                new TextPrompt<int>($"What is bank [fuchsia]interests[/] per year for deposit accounts? [fuchsia](%)[/]")
                    .PromptStyle("blue")
                    .Validate(depositInterestsPercent =>
                    {
                        return depositInterestsPercent switch
                        {
                            <= 0 => ValidationResult.Error("[red]Interests must be more than a 0[/]"),
                            >= 100 => ValidationResult.Error("[red]Interests must be less than 100[/]"),
                            _ => ValidationResult.Success(),
                        };
                    }));

            int depositInterestsLimit = AnsiConsole.Prompt(
                new TextPrompt<int>(
                        $"What is bank [fuchsia]limit[/] for {depositInterestsPercent} interests per year for deposit accounts?")
                    .PromptStyle("blue")
                    .Validate(depositInterestsLimit =>
                    {
                        return depositInterestsLimit switch
                        {
                            <= 0 => ValidationResult.Error("[red]Interests must be more than a 0[/]"),
                            _ => ValidationResult.Success(),
                        };
                    }));

            depositInterests.Add(new Tuple<Money, Percent>(new Money(depositInterestsLimit), new Percent(depositInterestsPercent)));
        }

        return depositInterests;
    }
}
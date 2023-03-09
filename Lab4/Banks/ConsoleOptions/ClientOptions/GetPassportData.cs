using Banks.Models;
using Spectre.Console;

namespace Banks.ConsoleOptions.ClientOptions;

public class GetPassportData
{
    public RussianPassportNumber Run()
    {
        var passportSeries = AnsiConsole.Prompt(
            new TextPrompt<int>("What is client's [fuchsia]passport series[/]?")
                .Validate(passportSeries => passportSeries is < 1000 or > 9999
                    ? ValidationResult.Error("Passport series consists from 4 numbers")
                    : ValidationResult.Success()));

        var passportNumber = AnsiConsole.Prompt(
            new TextPrompt<int>("What is client's [fuchsia]passport number[/]?")
                .Validate(passportNumber => passportNumber is < 100000 or > 999999
                    ? ValidationResult.Error("Passport number consists from 6 numbers")
                    : ValidationResult.Success()));

        return new RussianPassportNumber(passportSeries, passportNumber);
    }
}
using Banks.Models;
using Spectre.Console;

namespace Banks.ConsoleOptions.ClientOptions;

public class GetAddress
{
    public Address Run()
    {
        var index = AnsiConsole.Prompt(
            new TextPrompt<string>("What is client's address [fuchsia]index[/]?")
                .Validate(index =>
                {
                    return (index.Length != 6) || !int.TryParse(index, out int indexAsNumber)
                        ? ValidationResult.Error("Address index consists from 6 numbers")
                        : ValidationResult.Success();
                }));

        var city = AnsiConsole.Ask<string>("What is client's [fuchsia]city[/]?");
        var street = AnsiConsole.Ask<string>("What is client's [fuchsia]street[/]?");
        var building = AnsiConsole.Ask<string>("What is client's [fuchsia]building[/]?");
        var etc = AnsiConsole.Prompt(
            new TextPrompt<string>("[grey][[Optional]][/]What is client's etc address [fuchsia]information[/]?")
                .AllowEmpty());

        return new Address($"{index}, {city}, {street}, {building}, {etc}");
    }
}
using Banks.Entities;
using Spectre.Console;

namespace Banks.ConsoleOptions;

public class RewindTime
{
    public void Run()
    {
        var centralBank = CentralBank.GetInstance();

        var table1 = new Table();
        table1.AddColumn(new TableColumn("Name").Centered());
        table1.AddColumn(new TableColumn("Fund").Centered());

        foreach (var account in centralBank.ListOfBanks.SelectMany(x => x.ListOfBankAccounts))
        {
            table1.AddRow($"{account.Owner.Name}", $"{account.Fund}");
        }

        int years = AnsiConsole.Prompt(
            new TextPrompt<int>($"How many [fuchsia]years[/] from {centralBank.TimeMachine.CurDateTime}?")
                .PromptStyle("green")
                .Validate(years =>
                {
                    return years switch
                    {
                        < 0 => ValidationResult.Error("[red]Years number must be more than a 0[/]"),
                        _ => ValidationResult.Success(),
                    };
                }));

        int months = AnsiConsole.Prompt(
            new TextPrompt<int>($"How many [fuchsia]months[/] from {centralBank.TimeMachine.CurDateTime}?")
                .PromptStyle("green")
                .Validate(months =>
                {
                    return months switch
                    {
                        < 0 => ValidationResult.Error("[red]Months number must be more than a 0[/]"),
                        _ => ValidationResult.Success(),
                    };
                }));

        int days = AnsiConsole.Prompt(
            new TextPrompt<int>($"How many [fuchsia]days[/] from {centralBank.TimeMachine.CurDateTime}?")
                .PromptStyle("green")
                .Validate(days =>
                {
                    return days switch
                    {
                        < 0 => ValidationResult.Error("[red]Days number must be more than a 0[/]"),
                        _ => ValidationResult.Success(),
                    };
                }));

        if (centralBank.TimeMachine is RewindTimeMachine machine)
        {
            machine.RewindTime(years, months, days);
        }

        var table = new Table();
        table.AddColumn(new TableColumn("Name").Centered());
        table.AddColumn(new TableColumn("Fund").Centered());

        foreach (var account in centralBank.ListOfBanks.SelectMany(x => x.ListOfBankAccounts))
        {
            table.AddRow($"{account.Owner.Name}", $"{account.Fund}");
        }

        AnsiConsole.Write(table1);
        AnsiConsole.Write(table);
    }
}
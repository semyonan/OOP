using DataBaseAccess.Models;
using Spectre.Console;

namespace Reports.Test;

public class CreateReport
{
    public void Run()
    {
        var service = Services.GetInstance().MessageService;

        var sourceOptions = new List<string>();
        DateTime? start = null;
        DateTime? end = null;
        Guid? employee = null;

        var options = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                .Title("[fuchsia]Chose report statistics[/]?")
                .PageSize(10)
                .AddChoices(new[]
                {
                    "Whole number",
                    "Number by Source",
                    "Number by Time",
                    "Number by Employee",
                }));

        if (options.Contains("Number by Source"))
        {
            sourceOptions = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .Title("[fuchsia]Chose sources[/]?")
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                        "Email",
                        "Phone",
                        "Messenger",
                    }));
        }

        if (options.Contains("Number by Time"))
        {
            int hoursStart = AnsiConsole.Prompt(
                new TextPrompt<int>($"[fuchsia]Start hour[/]")
                    .PromptStyle("green")
                    .Validate(months =>
                    {
                        return months switch
                        {
                            < 0 => ValidationResult.Error("[red]Hours number must be more than a 0[/]"),
                            >= 24 => ValidationResult.Error("[red]Hours number must be less than a 24[/]"),
                            _ => ValidationResult.Success(),
                        };
                    }));

            int minutesStart = AnsiConsole.Prompt(
                new TextPrompt<int>($"[fuchsia]Start minutes[/]")
                    .PromptStyle("green")
                    .Validate(months =>
                    {
                        return months switch
                        {
                            < 0 => ValidationResult.Error("[red]Minutes number must be more than a 0[/]"),
                            >= 60 => ValidationResult.Error("[red]Minutes number must be less than a 60[/]"),
                            _ => ValidationResult.Success(),
                        };
                    }));

            int hoursEnd = AnsiConsole.Prompt(
                new TextPrompt<int>($"[fuchsia]End hour[/]")
                    .PromptStyle("green")
                    .Validate(months =>
                    {
                        return months switch
                        {
                            < 0 => ValidationResult.Error("[red]Hours number must be more than a 0[/]"),
                            >= 24 => ValidationResult.Error("[red]Hours number must be less than a 24[/]"),
                            _ => ValidationResult.Success(),
                        };
                    }));

            int minutesEnd = AnsiConsole.Prompt(
                new TextPrompt<int>($"[fuchsia]End minutes[/]")
                    .PromptStyle("green")
                    .Validate(months =>
                    {
                        return months switch
                        {
                            < 0 => ValidationResult.Error("[red]Minutes number must be more than a 0[/]"),
                            >= 60 => ValidationResult.Error("[red]Minutes number must be less than a 60[/]"),
                            _ => ValidationResult.Success(),
                        };
                    }));

            start = DateTime.Today + new TimeSpan(hoursStart, minutesStart, 0);
            end = DateTime.Today + new TimeSpan(hoursEnd, minutesEnd, 0);
        }

        if (options.Contains("Number by Employee"))
        {
            var res = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[fuchsia]Chose employee[/]?")
                    .PageSize(10)
                    .AddChoices(service.GetEmployees()));

            employee = Guid.Parse(service.GetWholeAccount(int.Parse(res.Split(' ')[0])));
        }

        service.CreateReport(options, sourceOptions, start, end, employee);
    }
}
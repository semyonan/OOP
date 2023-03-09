using Banks.Entities;
using Spectre.Console;

namespace Banks.ConsoleOptions.ClientOptions;

public class ChangeClient
{
    public void Run()
    {
        var option = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[fuchsia]Change or exit[/]?")
                .PageSize(10)
                .AddChoices(new[]
                {
                    "Change",
                    "Exit",
                }));

        var centralBank = CentralBank.GetInstance();

        var listOfClients = centralBank.ListOfClients.Where(x => (x.Address == null || x.PassportNumber == null))
            .Select(x => $"{x.Name} {x.SerName} {x.Id}").ToList();

        if (!listOfClients.Any())
        {
            AnsiConsole.WriteLine("There is no client to change");
            return;
        }

        while (option != "Exit")
        {
            var clientInfo = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[fuchsia]Which client do you want to change[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                    .AddChoices(listOfClients));

            var subs = clientInfo.Split(' ');
            var client = centralBank.GetClient(new Guid(subs[^1]));

            if ((client.Address == null) && AnsiConsole.Confirm($"Do you want to add address to {client.Name}?"))
            {
                client.AddAddress(new GetAddress().Run());
            }

            if ((client.PassportNumber == null) && AnsiConsole.Confirm($"Do you want to add passport number to {client.Name}?"))
            {
                client.AddPassportNumber(new GetPassportData().Run());
            }

            option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[fuchsia]Change or exit[/]?")
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                        "Change",
                        "Exit",
                    }));
        }
    }
}
using Banks.Entities;
using Banks.Models;
using Spectre.Console;

namespace Banks.ConsoleOptions.ClientOptions;

public class CreateClient
{
    public void Run()
    {
        var centralBank = CentralBank.GetInstance();

        var clientBuilder = new ClientBuilder();

        var name = AnsiConsole.Ask<string>("What is client's [fuchsia]name[/]?");
        clientBuilder.Name(name);

        var serName = AnsiConsole.Ask<string>($"What is {name}'s [fuchsia]serName[/]?");
        clientBuilder.SerName(serName);

        if (AnsiConsole.Confirm("Do you want to add passport data?"))
        {
            clientBuilder.PassportDate(new GetPassportData().Run());
        }

        if (AnsiConsole.Confirm("Do you want to enter full address?"))
        {
            clientBuilder.Address(new GetAddress().Run());
        }

        var client = clientBuilder.Build();

        centralBank.AddClient(
            client.Name,
            client.SerName,
            client.Address,
            client.PassportNumber,
            new ConsoleMessageHandler());
    }
}
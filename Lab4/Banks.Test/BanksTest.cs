using Banks.Entities;
using Banks.Exceptions;
using Banks.Models;
using Xunit;

namespace Banks.Test;

public class BanksTest
{
    [Fact]
    public void CreateClientAndMakeReplenishment()
    {
        var centralBank = new CentralBank(new RewindTimeMachine());

        var bank = centralBank.AddBank(
            "Сбербанк",
            new BankConfigurationBuilder()
                .DebitAccountInfo(new DebitAccountInfo(new Percent(10)))
                .DepositAccountInfo(new DepositAccountInfo(
                    new List<Tuple<Money, Percent>>() { new (new Money(0), new Percent(10)) }))
                .CreditAccountInfo(new CreditAccountInfo(new Percent(10), new Money(500000)))
                .LimitForPrecariousTransactions(new Money(1000))
                .Build());

        var clientId = centralBank.AddClient("Anna", "Семенова");

        var accountId = centralBank.AddDebitAccount(clientId, "Сбербанк");

        centralBank.ReplenishmentTransaction(accountId, new Money(500));

        Assert.True(centralBank.GetClient(clientId).ListOfAccounts[0].Fund == 500);
    }

    [Fact]
    public void RewindTime()
    {
        var centralBank = new CentralBank(new RewindTimeMachine());

        var bank = centralBank.AddBank(
            "Сбербанк",
            new BankConfigurationBuilder()
                .DebitAccountInfo(new DebitAccountInfo(new Percent(10)))
                .DepositAccountInfo(new DepositAccountInfo(
                    new List<Tuple<Money, Percent>>()
                    {
                        new (new Money(0), new Percent(10)),
                        new (new Money(1000), new Percent(20)),
                    }))
                .CreditAccountInfo(new CreditAccountInfo(new Percent(10), new Money(500000)))
                .LimitForPrecariousTransactions(new Money(1000))
                .Build());

        var clientId = centralBank.AddClient(
            "Anna",
            "Семенова",
            new Address("195426, Санкт-Петербург, ул.Ленская, д.313"),
            new RussianPassportNumber(2022, 102022));

        var debitAccountId = centralBank.AddDebitAccount(clientId, "Сбербанк");
        var depositAccountId = centralBank.AddDepositAccount(clientId, "Сбербанк", new TimeSpan(365, 0, 0, 0));

        centralBank.ReplenishmentTransaction(debitAccountId, new Money(1000));
        centralBank.ReplenishmentTransaction(depositAccountId, new Money(2000));

        if (centralBank.TimeMachine is RewindTimeMachine rewindTimeMachine)
        {
            rewindTimeMachine.RewindTime(1, 0, 0);
        }

        Assert.True(centralBank.GetAccount(debitAccountId).Fund is > 1100 and < 1150);
        Assert.True(centralBank.GetAccount(depositAccountId).Fund is > 2400 and < 2450);

        centralBank.WithdrawTransaction(depositAccountId, new Money(centralBank.GetAccount(depositAccountId).Fund));
    }

    [Fact]
    public void RemittanceWithAnotherBankCommission()
    {
        var centralBank = new CentralBank(new RewindTimeMachine());

        var bank1 = centralBank.AddBank(
            "Сбербанк",
            new BankConfigurationBuilder()
                .DebitAccountInfo(new DebitAccountInfo(new Percent(10)))
                .DepositAccountInfo(new DepositAccountInfo(
                    new List<Tuple<Money, Percent>>() { new (new Money(0), new Percent(10)) }))
                .CreditAccountInfo(new CreditAccountInfo(new Percent(10), new Money(500000)))
                .LimitForPrecariousTransactions(new Money(1000))
                .AnotherBankRemittanceCommission(new Percent(5))
                .Build());

        var bank2 = centralBank.AddBank(
            "Тинькофф",
            new BankConfigurationBuilder()
                .DebitAccountInfo(new DebitAccountInfo(new Percent(10)))
                .DepositAccountInfo(new DepositAccountInfo(
                    new List<Tuple<Money, Percent>>() { new (new Money(0), new Percent(10)) }))
                .CreditAccountInfo(new CreditAccountInfo(new Percent(10), new Money(500000)))
                .LimitForPrecariousTransactions(new Money(1000))
                .Build());

        var clientId = centralBank.AddClient(
            "Anna",
            "Семенова",
            new Address("195426, Санкт-Петербург, ул.Ленская, д.313"),
            new RussianPassportNumber(2022, 102022));

        var debitAccountId1 = centralBank.AddDebitAccount(clientId, "Сбербанк");
        var debitAccountId2 = centralBank.AddDebitAccount(clientId, "Тинькофф");

        centralBank.ReplenishmentTransaction(debitAccountId1, new Money(1000));
        centralBank.ReplenishmentTransaction(debitAccountId2, new Money(2000));

        centralBank.RemittanceTransaction(debitAccountId1, debitAccountId2, new Money(800));

        Assert.True(centralBank.GetAccount(debitAccountId1).Fund == 160);
        Assert.True(centralBank.GetAccount(debitAccountId2).Fund == 2800);
    }

    [Fact]
    public void ChangeBankAndGetNotification()
    {
        var centralBank = new CentralBank(new RewindTimeMachine());

        var bank = centralBank.AddBank(
            "Сбербанк",
            new BankConfigurationBuilder()
                .DebitAccountInfo(new DebitAccountInfo(new Percent(10)))
                .DepositAccountInfo(new DepositAccountInfo(
                    new List<Tuple<Money, Percent>>() { new (new Money(0), new Percent(10)) }))
                .CreditAccountInfo(new CreditAccountInfo(new Percent(10), new Money(500000)))
                .LimitForPrecariousTransactions(new Money(1000))
                .AnotherBankRemittanceCommission(new Percent(5))
                .Build());

        var clientId1 = centralBank.AddClient(
            "Anna",
            "Семенова",
            new Address("195426, Санкт-Петербург, ул.Ленская, д.313"),
            new RussianPassportNumber(2022, 102022),
            new StorageMessageHandler());

        var clientId2 = centralBank.AddClient(
            "Иван",
            "Иванов",
            new Address("195426, Санкт-Петербург, ул.Ленская, д.313"),
            new RussianPassportNumber(2021, 102022),
            new StorageMessageHandler());

        var debitAccountId1 = centralBank.AddDebitAccount(clientId1, "Сбербанк");
        var debitAccountId2 = centralBank.AddDebitAccount(clientId2, "Сбербанк");

        centralBank.GetBank("Сбербанк").CreateSubscribe(clientId1, Bank.NotifyOption.DebitInfo);
        centralBank.GetBank("Сбербанк").CreateSubscribe(clientId2, Bank.NotifyOption.Withdrawal);

        centralBank.GetBank("Сбербанк").ChangeDebitAccountInfo(new Percent(15));
        centralBank.GetBank("Сбербанк").ChangeWithdrawalCommission(new Percent(20), new Money(2000));

        var handler1 = centralBank.GetClient(clientId1).MessageHandler as StorageMessageHandler;
        var handler2 = centralBank.GetClient(clientId2).MessageHandler as StorageMessageHandler;

        Assert.True(handler1?.Messages.Count == 1);
        Assert.True(handler2?.Messages.Count == 1);
    }

    [Fact]
    public void TryToWithdrawFromDepositAccountBeforeTermEnded()
    {
        var centralBank = new CentralBank(new RewindTimeMachine());

        var bank = centralBank.AddBank(
            "Сбербанк",
            new BankConfigurationBuilder()
                .DebitAccountInfo(new DebitAccountInfo(new Percent(10)))
                .DepositAccountInfo(new DepositAccountInfo(
                    new List<Tuple<Money, Percent>>() { new (new Money(0), new Percent(10)) }))
                .CreditAccountInfo(new CreditAccountInfo(new Percent(10), new Money(500000)))
                .LimitForPrecariousTransactions(new Money(1000))
                .Build());

        var clientId = centralBank.AddClient(
            "Anna",
            "Семенова",
            new Address("195426, Санкт-Петербург, ул.Ленская, д.313"),
            new RussianPassportNumber(2022, 102022));

        var depositAccountId = centralBank.AddDepositAccount(clientId, "Сбербанк", new TimeSpan(365, 0, 0, 0));

        centralBank.ReplenishmentTransaction(depositAccountId, new Money(500));

        Assert.Throws<TransactionException>(() => centralBank.WithdrawTransaction(depositAccountId, new Money(500)));
    }

    [Fact]
    public void RemittanceReceiverAccount_TrowsException()
    {
        var centralBank = new CentralBank(new RewindTimeMachine());

        var bank = centralBank.AddBank(
            "Сбербанк",
            new BankConfigurationBuilder()
                .DebitAccountInfo(new DebitAccountInfo(new Percent(10)))
                .DepositAccountInfo(new DepositAccountInfo(
                    new List<Tuple<Money, Percent>>() { new (new Money(0), new Percent(10)) }))
                .CreditAccountInfo(new CreditAccountInfo(new Percent(10), new Money(500000)))
                .LimitForPrecariousTransactions(new Money(1000))
                .Build());

        var clientId = centralBank.AddClient(
            "Anna",
            "Семенова",
            new Address("195426, Санкт-Петербург, ул.Ленская, д.313"),
            new RussianPassportNumber(2022, 102022));

        var depositAccountId = centralBank.AddDepositAccount(clientId, "Сбербанк", new TimeSpan(365, 0, 0, 0));
        var debitAccountId = centralBank.AddDebitAccount(clientId, "Сбербанк");

        centralBank.ReplenishmentTransaction(debitAccountId, new Money(1000));
        centralBank.ReplenishmentTransaction(depositAccountId, new Money(2000));

        if (centralBank.TimeMachine is RewindTimeMachine rewindTimeMachine)
        {
            rewindTimeMachine.RewindTime(1, 0, 0);
        }

        var fundBefore = centralBank.GetAccount(debitAccountId).Fund;

        try
        {
            centralBank.RemittanceTransaction(debitAccountId, depositAccountId, new Money(1000));
        }
        catch (System.Exception)
        {
            Assert.True(centralBank.GetAccount(debitAccountId).Fund == fundBefore);
        }
    }
}
using Banks.Exceptions;
using Banks.Models;

namespace Banks.Entities;

public class Bank : IEquatable<Bank>
{
    private readonly List<IBankAccount> _listOfBankAccounts;

    public Bank(
        string name,
        BankConfiguration bankConfiguration)
    {
        Name = name;
        BankConfiguration = bankConfiguration;
        _listOfBankAccounts = new List<IBankAccount>();
    }

    public delegate void MessageSender(string message);

    public event MessageSender? DebitInfoNotify;
    public event MessageSender? DepositInfoNotify;
    public event MessageSender? CreditInfoNotify;
    public event MessageSender? RemittanceInfoNotify;
    public event MessageSender? WithdrawalInfoNotify;

    public enum NotifyOption
    {
        DebitInfo,
        DepositInfo,
        CreditInfo,
        Remittance,
        Withdrawal,
    }

    public string Name { get; }
    public BankConfiguration BankConfiguration { get; }

    public IReadOnlyList<IBankAccount> ListOfBankAccounts => _listOfBankAccounts.AsReadOnly();

    public void AddAccount(IBankAccount bankAccount)
    {
        _listOfBankAccounts.Add(bankAccount);
    }

    public void RecountInterests()
    {
        foreach (var account in _listOfBankAccounts)
        {
            if (account is DebitAccount debitAccount)
            {
                debitAccount.InterestsCounter.Increase(
                    BankConfiguration.DebitAccountInfo.InterestsPerDay,
                    Math.Abs(debitAccount.Fund));
            }

            if (account is DepositAccount depositAccount)
            {
                if (!depositAccount.Opened)
                {
                    var interests = BankConfiguration.DepositAccountInfo.InterestsPerDay
                        .FirstOrDefault(x => x.Item1.Sum > depositAccount.Fund);

                    if (interests == null)
                    {
                        depositAccount.InterestsCounter
                            .Increase(BankConfiguration.DepositAccountInfo.InterestsPerDay[^1].Item2, Math.Abs(depositAccount.Fund));
                    }

                    if (interests != null)
                    {
                        depositAccount.InterestsCounter.Increase(interests.Item2, Math.Abs(depositAccount.Fund));
                    }
                }
            }
        }
    }

    public void PayInterests()
    {
        foreach (var account in _listOfBankAccounts)
        {
            if (account is IInterestsContaining interestsContainingAccount)
            {
                account.SpecificBankReplenishment(new Money((decimal)interestsContainingAccount.InterestsCounter.Interest));
                interestsContainingAccount.InterestsCounter.Reset();
            }
        }
    }

    public void CheckTermableAccounts(DateTime curDateTime)
    {
        var termEndedAccounts = _listOfBankAccounts
            .Where(x => x is ITermableAccount termableAccount &&
                        (termableAccount.Start + termableAccount.Term) == curDateTime).ToList();

        foreach (var accounts in termEndedAccounts)
        {
            if (accounts is ITermableAccount termableAccount)
            {
                termableAccount.Open();
            }
        }
    }

    public void CreateSubscribe(Guid clientId, NotifyOption option)
    {
        var account = _listOfBankAccounts.FirstOrDefault(
            x => x.Owner.Id == clientId);

        if (account == null)
        {
            throw new BankException("Client have no accounts in this bank");
        }

        var client = account.Owner;

        switch (option)
        {
            case NotifyOption.DebitInfo:
            {
                DebitInfoNotify += client.MessageHandler.Handle;
                break;
            }

            case NotifyOption.DepositInfo:
            {
                DepositInfoNotify += client.MessageHandler.Handle;
                break;
            }

            case NotifyOption.CreditInfo:
            {
                CreditInfoNotify += client.MessageHandler.Handle;
                break;
            }

            case NotifyOption.Remittance:
            {
                RemittanceInfoNotify += client.MessageHandler.Handle;
                break;
            }

            case NotifyOption.Withdrawal:
            {
                WithdrawalInfoNotify += client.MessageHandler.Handle;
                break;
            }
        }
    }

    public void CreateUnsubscribe(Guid clientId, NotifyOption option)
    {
        var account = _listOfBankAccounts.FirstOrDefault(
            x => x.Owner.Id == clientId);

        if (account == null)
        {
            throw new BankException("Client have no accounts in this bank");
        }

        var client = account.Owner;

        switch (option)
        {
            case NotifyOption.DebitInfo:
            {
                if (DebitInfoNotify == null || !DebitInfoNotify.GetInvocationList().Contains(client.MessageHandler.Handle))
                {
                    throw new Exception();
                }

                DebitInfoNotify -= client.MessageHandler.Handle;

                break;
            }

            case NotifyOption.DepositInfo:
            {
                if (DepositInfoNotify == null || !DepositInfoNotify.GetInvocationList().Contains(client.MessageHandler.Handle))
                {
                    throw new Exception();
                }

                DepositInfoNotify -= client.MessageHandler.Handle;
                break;
            }

            case NotifyOption.CreditInfo:
            {
                if (CreditInfoNotify == null || !CreditInfoNotify.GetInvocationList().Contains(client.MessageHandler.Handle))
                {
                    throw new Exception();
                }

                CreditInfoNotify -= client.MessageHandler.Handle;
                break;
            }

            case NotifyOption.Remittance:
            {
                if (RemittanceInfoNotify == null || !RemittanceInfoNotify.GetInvocationList().Contains(client.MessageHandler.Handle))
                {
                    throw new Exception();
                }

                RemittanceInfoNotify -= client.MessageHandler.Handle;
                break;
            }

            case NotifyOption.Withdrawal:
            {
                if (WithdrawalInfoNotify == null || !WithdrawalInfoNotify.GetInvocationList().Contains(client.MessageHandler.Handle))
                {
                    throw new Exception();
                }

                WithdrawalInfoNotify -= client.MessageHandler.Handle;
                break;
            }
        }
    }

    public void ChangeDebitAccountInfo(Percent percent)
    {
        BankConfiguration.ChangeDebitAccountInfo(new DebitAccountInfo(percent));
        DebitInfoNotify?.Invoke($"Bank {Name} changed interests for debit accounts to {percent} per year");
    }

    public void ChangeDepositAccountInfo(List<Tuple<Money, Percent>> interests)
    {
        interests.Sort((x, y) => x.Item1.CompareTo(y.Item1));
        BankConfiguration.ChangeDepositAccountInfo(new DepositAccountInfo(interests));
        DepositInfoNotify?.Invoke($"Bank {Name} changed interests for deposit accounts to {interests[0].Item2} per year");
    }

    public void ChangeCreditAccountInfo(Percent commission, Money limit)
    {
        BankConfiguration.ChangeCreditAccountInfo(new CreditAccountInfo(commission, limit));
        CreditInfoNotify?.Invoke($"Bank {Name} changed commission for credit accounts to {commission} per year");
    }

    public void ChangeRemittanceCommission(Percent commission)
    {
        BankConfiguration.ChangeAnotherBankRemittanceCommission(commission);
        RemittanceInfoNotify?.Invoke($"Bank {Name} changed commission for remittance to another banks to {commission}");
    }

    public void ChangeWithdrawalCommission(Percent commission, Money limit)
    {
        BankConfiguration.ChangeWithdrawalCommission(commission, limit);
        WithdrawalInfoNotify?.Invoke($"Bank {Name} changed commission for withdrawal to {commission} if sum is more than {limit}");
    }

    public void ChangeLimitForPrecariousTransactions(Money limit)
    {
        BankConfiguration.ChangeLimitForPrecariousTransactions(limit);
    }

    public bool Equals(Bank? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Bank)obj);
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }

    private bool AccountExists(Guid id) => _listOfBankAccounts.Exists(x => x.Id == id);
    private IBankAccount? FindAccount(Guid id) => _listOfBankAccounts.FirstOrDefault(x => x.Id == id);
}
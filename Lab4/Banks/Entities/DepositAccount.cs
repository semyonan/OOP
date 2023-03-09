using Banks.Exceptions;
using Banks.Models;

namespace Banks.Entities;

public class DepositAccount : IBankAccount, IInterestsContaining, ITermableAccount
{
    public DepositAccount(Client owner, Bank bank, DateTime start, TimeSpan term)
    {
        Id = Guid.NewGuid();
        Owner = owner;
        Bank = bank;
        Fund = 0;
        Start = start;
        Term = term;
        Opened = false;
        InterestsCounter = new InterestsCounter();
    }

    public Guid Id { get; }
    public Client Owner { get; }
    public Bank Bank { get; }
    public decimal Fund { get; private set; }
    public InterestsCounter InterestsCounter { get; }
    public DateTime Start { get; }
    public TimeSpan Term { get; }
    public bool Opened { get; private set; }

    public void Open()
    {
        Opened = true;
    }

    public void Withdrawal(Money value)
    {
        if (!Opened)
        {
            throw new TransactionException("Deposit term not ended yet");
        }

        if (Fund < value.Sum)
        {
            throw new TransactionException("Insufficient funds");
        }

        Fund -= value.Sum;
    }

    public void Replenishment(Money value)
    {
        if (Opened)
        {
            throw new TransactionException("Deposit term ended");
        }

        Fund += value.Sum;
    }

    public void SendRemittance(Money value)
    {
        if (!Opened)
        {
            throw new TransactionException("Deposit term not ended yet");
        }

        if (Fund < value.Sum)
        {
            throw new TransactionException("Insufficient funds");
        }

        Fund -= value.Sum;
    }

    public void ReceiveRemittance(Money value)
    {
        if (Opened)
        {
            throw new TransactionException("Deposit term ended");
        }

        Fund += value.Sum;
    }

    public void SpecificBankWithdrawal(Money value)
    {
        Fund -= value.Sum;
    }

    public void SpecificBankReplenishment(Money value)
    {
        Fund += value.Sum;
    }
}
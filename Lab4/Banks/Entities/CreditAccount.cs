using Banks.Exceptions;
using Banks.Models;

namespace Banks.Entities;

public class CreditAccount : IBankAccount
{
    public CreditAccount(Client owner, Bank bank, Money limit)
    {
        Id = Guid.NewGuid();
        Owner = owner;
        Limit = limit;
        Bank = bank;
        Fund = 0;
    }

    public Guid Id { get; }
    public Client Owner { get; }
    public Bank Bank { get; }
    public decimal Fund { get; private set; }
    public Money Limit { get; }

    public void Withdrawal(Money value)
    {
        if (value.Sum - Fund < -Limit.Sum)
        {
            throw new TransactionException("Insufficient funds");
        }

        Fund -= value.Sum;
    }

    public void Replenishment(Money value)
    {
        Fund += value.Sum;
    }

    public void SendRemittance(Money value)
    {
        if (Fund - value.Sum < -Limit.Sum)
        {
            throw new TransactionException("Insufficient funds");
        }

        Fund -= value.Sum;
    }

    public void ReceiveRemittance(Money value)
    {
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
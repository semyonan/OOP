using Banks.Exceptions;
using Banks.Models;

namespace Banks.Entities;

public class DebitAccount : IBankAccount, IInterestsContaining
{
    public DebitAccount(Client owner, Bank bank)
    {
        Id = Guid.NewGuid();
        Owner = owner;
        Bank = bank;
        Fund = 0;
        InterestsCounter = new InterestsCounter();
    }

    public Guid Id { get; }
    public Client Owner { get; }
    public Bank Bank { get; }
    public decimal Fund { get; private set; }
    public InterestsCounter InterestsCounter { get; }

    public void Withdrawal(Money value)
    {
        if (Fund < value.Sum)
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
        if (Fund < value.Sum)
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
using Banks.Models;

namespace Banks.Entities;

public class WithdrawTransaction : ITransaction
{
    public WithdrawTransaction(IBankAccount bankAccount, Money sum)
    {
        BankAccount = bankAccount;
        Id = Guid.NewGuid();
        Sum = sum;
    }

    public IBankAccount BankAccount { get; }
    public Guid Id { get; }
    public Money Sum { get; }
}
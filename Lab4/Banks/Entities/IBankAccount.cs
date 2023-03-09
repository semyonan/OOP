using Banks.Models;

namespace Banks.Entities;

public interface IBankAccount
{
    public Guid Id { get; }
    public Client Owner { get; }
    public Bank Bank { get; }
    public decimal Fund { get; }

    public void Withdrawal(Money value);
    public void Replenishment(Money value);
    public void SendRemittance(Money value);
    public void ReceiveRemittance(Money value);
    public void SpecificBankWithdrawal(Money value);
    public void SpecificBankReplenishment(Money value);
}
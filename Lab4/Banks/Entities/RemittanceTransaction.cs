using Banks.Models;

namespace Banks.Entities;

public class RemittanceTransaction : ITransaction
{
    public RemittanceTransaction(IBankAccount senderAccount, IBankAccount receiverAccount, Money sum)
    {
        SenderAccount = senderAccount;
        ReceiverAccount = receiverAccount;
        Id = Guid.NewGuid();
        Sum = sum;
    }

    public IBankAccount SenderAccount { get; }
    public IBankAccount ReceiverAccount { get; }
    public Guid Id { get; }
    public Money Sum { get; }
}
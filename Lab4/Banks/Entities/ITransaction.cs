using Banks.Models;

namespace Banks.Entities;

public interface ITransaction
{
    public Guid Id { get; }
    public Money Sum { get; }
}
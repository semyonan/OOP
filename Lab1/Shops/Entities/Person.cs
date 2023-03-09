using Shops.Exceptions;

namespace Shops.Entities;

public class Person
{
    public Person(string name, decimal balance)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new PersonException("Invalid person name");
        }

        if (balance < 0)
        {
            throw new PersonException("Balance can't be under zero");
        }

        Name = name;
        Balance = balance;
    }

    public string Name { get; }
    public decimal Balance { get; private set; }

    public void DecreaseBalanceAfterBuying(decimal purchaseCost)
    {
        if (purchaseCost > Balance)
        {
            throw new ShopException("Balance can't be under zero");
        }

        Balance -= purchaseCost;
    }

    public void IncreaseBalanceAfterReplenishment(decimal replenishmentValue)
    {
        if (replenishmentValue <= 0)
        {
            throw new ShopException("Replenishment can't be under zero");
        }

        Balance += replenishmentValue;
    }
}
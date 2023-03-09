using Shops.Exceptions;
using Shops.Models;

namespace Shops.Entities;

public class Product : IComparable
{
    public Product(Item item, decimal price, uint amount)
    {
        Item = item;
        Price = new Price(price);
        Amount = amount;
    }

    public Item Item { get; }
    public Price Price { get; private set; }
    public uint Amount { get; private set; }

    public void ChangePrice(Price newPrice)
    {
        Price = newPrice;
    }

    public void IncreaseAmountAfterDelivery(uint deliveryCount)
    {
        Amount += deliveryCount;
    }

    public void DecreaseAmountAfterBuying(uint boughtCount)
    {
        Amount -= boughtCount;
    }

    public override int GetHashCode()
    {
        return Item.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj is Product product)
        {
            return product.Item == Item;
        }

        return false;
    }

    public int CompareTo(object? obj)
    {
        if (obj is not Product product)
        {
            throw new ProductException("Invalid argument");
        }

        return Price.CompareTo(product.Price);
    }
}
using Shops.Exceptions;

namespace Shops.Models;

public class Item
{
    public Item(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ItemException("Invalid product name");
        }

        Name = name.ToLower();
        Id = Guid.NewGuid();
    }

    public string Name { get; }
    public Guid Id { get; }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj is Item item)
        {
            return item.Id == Id;
        }

        return false;
    }
}
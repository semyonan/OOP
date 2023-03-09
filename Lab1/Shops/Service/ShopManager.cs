using Shops.Entities;
using Shops.Exceptions;
using Shops.Models;

namespace Shops.Service;

public class ShopManager
{
    private readonly List<Person> _personList = new List<Person>();
    private readonly List<Item> _itemList = new List<Item>();
    private readonly List<Shop> _shopList = new List<Shop>();

    public Person AddPerson(string name, decimal balance)
    {
        var person = new Person(name, balance);

        _personList.Add(person);

        return person;
    }

    public Item AddItem(string name)
    {
        var item = new Item(name);

        _itemList.Add(item);

        return item;
    }

    public Shop AddShop(string name, string address)
    {
        var shop = new Shop(name, address);

        _shopList.Add(shop);

        return shop;
    }

    public void RemoveShop(Guid id)
    {
        Shop currentShop = GetShopById(id);

        _shopList.Remove(currentShop);
    }

    public void Delivery(Shop shop, List<Product> rangeOfProducts)
    {
        if (!ShopExists(shop))
        {
            throw new ServiceException("This shop doesn't exists");
        }

        shop.AddRangeOfProducts(rangeOfProducts);
    }

    public Shop FindShopByProduct(Product product)
    {
        Shop? currentShop = _shopList.FirstOrDefault(s => s.ProductList.Contains(product));

        if (currentShop == null)
        {
            throw new ServiceException("There is no shop, which contains this product");
        }

        return currentShop;
    }

    public Shop GetShopByName(string name)
    {
        Shop? shop = FindShopByName(name);

        if (shop is null)
        {
            throw new ServiceException("Shop doesn't exists");
        }

        return shop;
    }

    public Shop? FindShopByName(string name)
    {
        return _shopList.FirstOrDefault(x => x.Name == name);
    }

    public Shop GetShopById(Guid id)
    {
        Shop? shop = FindShopById(id);

        if (shop is null)
        {
            throw new ServiceException("Shop doesn't exists");
        }

        return shop;
    }

    public Shop? FindShopById(Guid id)
    {
        return _shopList.FirstOrDefault(x => x.Id == id);
    }

    public Product FindMinCostProduct(string nameOfProduct, uint amount)
    {
        var selectedProduts = _shopList.SelectMany(s => s.ProductList.Where(p => p.Item.Name.Contains(nameOfProduct.ToLower()) && p.Amount >= amount))
            .ToList();

        var minCostProduct = selectedProduts.Min();

        if (minCostProduct == null)
        {
            throw new ServiceException("There is no appropriate product");
        }

        return minCostProduct;
    }

    public void BuyProduct(Person person, string nameOfProduct, uint amount)
    {
        var minCostProduct = FindMinCostProduct(nameOfProduct, amount);

        if (person.Balance < amount * minCostProduct.Price.Value)
        {
            throw new ServiceException("Person doesn't have enough money");
        }

        minCostProduct.DecreaseAmountAfterBuying(amount);
        person.DecreaseBalanceAfterBuying(minCostProduct.Price.Value * amount);

        if (minCostProduct.Amount == 0)
        {
            Shop currentShop = FindShopByProduct(minCostProduct);
            currentShop.RemoveProduct(minCostProduct);
        }
    }

    public void BuyRangeOfProducts(Person person, List<Tuple<string, uint>> rangeOfProducts)
    {
        foreach (Tuple<string, uint> product in rangeOfProducts)
        {
            BuyProduct(person, product.Item1, product.Item2);
        }
    }

    public Person BalanceReplenishment(Person person, decimal replenishmentValue)
    {
        if (!PersonExists(person))
        {
            throw new ServiceException("Person doesn't exists");
        }

        person.IncreaseBalanceAfterReplenishment(replenishmentValue);

        return person;
    }

    public void ChangeProductPrices(Shop shop, List<Tuple<Product, decimal>> productsToChange)
    {
        if (!ShopExists(shop))
        {
            throw new ServiceException("This shop doesn't exists");
        }

        shop.ChangeProductPrices(productsToChange);
    }

    private bool ShopExists(Shop shop) => _shopList.Exists(x => x.Equals(shop));
    private bool PersonExists(Person person) => _personList.Exists(x => x.Equals(person));
}
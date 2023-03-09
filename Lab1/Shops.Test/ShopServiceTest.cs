using Shops.Entities;
using Shops.Exceptions;
using Shops.Models;
using Shops.Service;
using Xunit;

namespace Shops.Test;

public class ShopServiceTest
{
    [Fact]
    public void AddShop_MakeDelivery()
    {
        var shops = new ShopManager();

        Shop shop1 = shops.AddShop("Nana", "195426, Санкт-Петербург, ул.Соликамская, д.313");

        Item item1 = shops.AddItem("Dress");
        Item item2 = shops.AddItem("Blue Dress");
        Item item3 = shops.AddItem("Red Dress");
        shops.Delivery(shop1, new List<Product> { new Product(item1, 10, 5), new Product(item2, 10, 4), new Product(item3, 100, 4) });
        shops.Delivery(shop1, new List<Product> { new Product(item1, 100, 5), new Product(item2, 1000, 4), new Product(item3, 1000, 4) });

        Assert.True(shop1.ProductList[0].Amount == 10 && shop1.ProductList[0].Price.Value == 100);
        Assert.True(shop1.ProductList[1].Amount == 8 && shop1.ProductList[1].Price.Value == 1000);
        Assert.True(shop1.ProductList[2].Amount == 8 && shop1.ProductList[2].Price.Value == 1000);
    }

    [Fact]
    public void FindMinCostProductAndBuyIt()
    {
        var shops = new ShopManager();

        Shop shop1 = shops.AddShop("Nana", "195426, Санкт-Петербург, ул.Соликамская, д.313");
        Shop shop2 = shops.AddShop("Blue", "109876, Санкт-Петербург, Невский проспект, д.3, к.3");

        Person person = shops.AddPerson("Вася", 100000);

        Item item1 = shops.AddItem("Dress");
        Item item2 = shops.AddItem("Blue Dress");
        Item item3 = shops.AddItem("Red Dress");

        var product = new Product(item2, 10, 4);

        shops.Delivery(shop1, new List<Product> { new Product(item1, 10, 5), new Product(item2, 1000, 4), new Product(item3, 100, 4) });
        shops.Delivery(shop2, new List<Product> { new Product(item1, 100, 5), product, new Product(item3, 1000, 4) });

        Assert.True(shops.FindMinCostProduct("blue dress", 4).Equals(product));

        shops.BuyProduct(person, "blue dress", 3);

        Assert.True(product.Amount == 1);
    }

    [Fact]
    public void CreatePerson_BuyRangeOfProducts()
    {
        var shops = new ShopManager();

        Shop shop1 = shops.AddShop("Nana", "195426, Санкт-Петербург, ул.Осипенко, д.313");
        Shop shop2 = shops.AddShop("Blue", "109876, Санкт-Петербург, Невский проспект, д.3, к.3");

        Person person = shops.AddPerson("Вася", 100000);

        Item item1 = shops.AddItem("Dress");
        Item item2 = shops.AddItem("Blue Dress");
        Item item3 = shops.AddItem("Red Dress");

        shops.Delivery(shop1, new List<Product> { new Product(item1, 100, 5), new Product(item2, 1000, 4), new Product(item3, 1000, 4) });
        shops.Delivery(shop2, new List<Product> { new Product(item1, 1000, 5), new Product(item2, 100, 4), new Product(item3, 10, 4) });

        shops.BuyRangeOfProducts(person, new List<Tuple<string, uint>> { new Tuple<string, uint>("dress", 3), new Tuple<string, uint>("blue dress", 3) });

        Assert.True(shop2.ProductList[2].Amount == 1);
        Assert.True(shop2.ProductList[1].Amount == 1);
        Assert.True(person.Balance == 99670);
    }

    [Fact]
    public void PersonDoesNotHaveEnoughMoney_ThrowException()
    {
        var shops = new ShopManager();

        Shop shop1 = shops.AddShop("Nana", "195426, Санкт-Петербург, ул.Осипенко, д.313");
        Shop shop2 = shops.AddShop("Blue", "109876, Санкт-Петербург, Невский проспект, д.3, к.3");

        Person person = shops.AddPerson("Вася", 300);

        Item item1 = shops.AddItem("Dress");
        Item item2 = shops.AddItem("Blue Dress");
        Item item3 = shops.AddItem("Red Dress");

        shops.Delivery(shop1, new List<Product> { new Product(item1, 100, 5), new Product(item2, 1000, 4), new Product(item3, 1000, 4) });
        shops.Delivery(shop2, new List<Product> { new Product(item1, 1000, 5), new Product(item2, 100, 4), new Product(item3, 10, 4) });

        Assert.Throws<ServiceException>(() => shops.BuyRangeOfProducts(person, new List<Tuple<string, uint>> { new Tuple<string, uint>("dress", 3), new Tuple<string, uint>("blue dress", 3) }));
    }

    [Fact]
    public void TooMuchProductToBuy_ThrowException()
    {
        var shops = new ShopManager();

        Shop shop1 = shops.AddShop("Nana", "195426, Санкт-Петербург, ул.Осипенко, д.313");
        Shop shop2 = shops.AddShop("Blue", "109876, Санкт-Петербург, Невский проспект, д.3, к.3");

        Person person = shops.AddPerson("Вася", 10000);

        Item item1 = shops.AddItem("Dress");
        Item item2 = shops.AddItem("Blue Dress");
        Item item3 = shops.AddItem("Red Dress");

        shops.Delivery(shop1, new List<Product> { new Product(item1, 100, 5), new Product(item2, 1000, 4), new Product(item3, 1000, 4) });
        shops.Delivery(shop2, new List<Product> { new Product(item1, 1000, 5), new Product(item2, 100, 4), new Product(item3, 10, 4) });

        Assert.Throws<ServiceException>(() => shops.BuyRangeOfProducts(person, new List<Tuple<string, uint>> { new Tuple<string, uint>("dress", 3), new Tuple<string, uint>("blue dress", 10) }));
    }
}
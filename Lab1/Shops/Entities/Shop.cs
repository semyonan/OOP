using Shops.Exceptions;
using Shops.Models;

namespace Shops.Entities;

public class Shop
{
    private List<Product> _productList;
    public Shop(string name, string address)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ShopException("Invalid shop name");
        }

        Name = name;
        Id = Guid.NewGuid();
        Address = new Address(address);
        _productList = new List<Product>();
    }

    public string Name { get; }
    public Guid Id { get; }
    public Address Address { get; }

    public IReadOnlyList<Product> ProductList => _productList.AsReadOnly();

    public void RemoveProduct(Product product)
    {
        if (!ProductExists(product))
        {
            throw new ShopException("Product doesn't exists at this shop");
        }

        _productList.Remove(product);
    }

    public void AddRangeOfProducts(List<Product> rangeOfProducts)
    {
        var productsToChange = _productList.Intersect(rangeOfProducts).ToList();

        foreach (var product in productsToChange)
        {
            var deliveredProduct = rangeOfProducts.FirstOrDefault(x => x.Item.Equals(product.Item));
            if (deliveredProduct == null)
            {
                throw new ShopException("Can't find product in shop");
            }

            product.IncreaseAmountAfterDelivery(deliveredProduct.Amount);
            product.ChangePrice(deliveredProduct.Price);
        }

        var productsToAdd = rangeOfProducts.Except(productsToChange).ToList();
        _productList.AddRange(productsToAdd);
    }

    public void ChangeProductPrices(List<Tuple<Product, decimal>> productsWithNewPrices)
    {
        foreach (var productToChange in productsWithNewPrices)
        {
            if (!ProductExists(productToChange.Item1))
            {
                throw new ShopException("Product, which you want to change, doesn't exists");
            }

            productToChange.Item1.ChangePrice(new Price(productToChange.Item2));
        }
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj is Shop shop)
        {
            return shop.Id == Id;
        }

        return false;
    }

    private bool ProductExists(Product product) => _productList.Exists(x => x.Equals(product));
}
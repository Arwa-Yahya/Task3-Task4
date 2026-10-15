
namespace Products.Domain.Entities
{
    public class Product
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public decimal Price { get; private set; }

        private Product() { }

        public Product(string name, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name is required.", nameof(name));

            if (price <= 0)
                throw new ArgumentOutOfRangeException(nameof(price), "Product price must be greater than zero.");

            Name = name.Trim();
            Price = price;
        }
    }
}

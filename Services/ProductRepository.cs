using ViewModelValidation.Models;

namespace ViewModelValidation.Services;

/// <summary>Жадтағы (in-memory) тауар репозиторийі – оқу мақсатында деректер қорының орнына.</summary>
public class ProductRepository
{
    private readonly object _lock = new();
    private readonly List<Product> _products = new()
    {
        new Product { Id = 1, Name = "Ноутбук Lenovo IdeaPad", Sku = "LAP-1001", Category = "Электроника",
            Price = 320000m, Stock = 12, SupplierEmail = "supply@lenovo.kz", CreatedAt = DateTime.Now.AddDays(-10) },
        new Product { Id = 2, Name = "Қысқы күртеше", Sku = "JKT-2002", Category = "Киім",
            Price = 45000m, DiscountPrice = 39900m, Stock = 30, SupplierEmail = "info@almaty-wear.kz", CreatedAt = DateTime.Now.AddDays(-5) },
        new Product { Id = 3, Name = "C# программалау кітабы", Sku = "BOK-3003", Category = "Кітаптар",
            Price = 8500m, Stock = 50, SupplierEmail = "books@kitap.kz", CreatedAt = DateTime.Now.AddDays(-2) }
    };
    private int _nextId = 4;

    public List<Product> GetAll()
    {
        lock (_lock)
        {
            return _products.OrderByDescending(p => p.Id).ToList();
        }
    }

    public bool SkuExists(string sku)
    {
        lock (_lock)
        {
            return _products.Any(p => p.Sku.Equals(sku, StringComparison.OrdinalIgnoreCase));
        }
    }

    public Product Add(Product product)
    {
        lock (_lock)
        {
            product.Id = _nextId++;
            _products.Add(product);
            return product;
        }
    }
}

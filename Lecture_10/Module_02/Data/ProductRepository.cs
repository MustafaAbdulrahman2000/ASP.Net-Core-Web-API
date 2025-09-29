using Module_02.Models;

namespace Module_02.Data;

public class ProductRepository
{
    private readonly List<Product> _products = 
    [
       new Product { Id = Guid.Parse("2779ee47-10b0-4bd7-8342-404006aa1392"), Name = "Soda", Price = 1.99m }
    ];

    public Product? GetProductById(Guid productId)
    {
        var product = _products.FirstOrDefault(p => p.Id == productId);

        return product ?? null;
    }
}
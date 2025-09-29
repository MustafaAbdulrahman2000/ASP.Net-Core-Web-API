using  Module_02.Models;

namespace Module_02.Responses.V1;

public class ProductResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }

    private ProductResponse() { }

    public static ProductResponse FromModel(Product product)
    {
        if (product is null)
            throw new ArgumentNullException(nameof(product), "Cannot create a response from a null product.");

        var response = new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        };

        return response;
    }
    public static IEnumerable<ProductResponse> FromModels(IEnumerable<Product> products)
    {
        if (products == null)
            throw new ArgumentNullException(nameof(products), "Cannot create responses from a null collection");

        return products.Select(p => FromModel(p));
    }

}
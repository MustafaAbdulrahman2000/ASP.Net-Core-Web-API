using Module_01.Models;

namespace Module_01.Responses.V2;

public class ProductResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public PriceResponse Price { get; set; } = null!;

    private ProductResponse() { }

    public static ProductResponse FromModel(Product product)
    {
        if (product is null)
            throw new ArgumentNullException(nameof(product), "Cannot create a response from a null product.");

        var response = new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Price = new PriceResponse
            {
                Amount = product.Price,
                Currency = "USD"
            }
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
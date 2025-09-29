using Module_01.Models;

namespace Module_01.Responses;

public class ProductResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public List<ProductReviewResponse>? Reviews { get; set; }

    private ProductResponse() {}

    public static ProductResponse FromModel(Product product, IEnumerable<ProductReview> reviews = null!)
    {
        if (product is null)
            throw new ArgumentNullException(nameof(product), "Cannot create a response from a null product.");

        var response = new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        };

        if (reviews is not null)
            response.Reviews = ProductReviewResponse.FromModels(reviews).ToList();

        return response;
    }
    public static IEnumerable<ProductResponse> FromModels(IEnumerable<Product> products)
    {
        if (products == null)
            throw new ArgumentNullException(nameof(products), "Cannot create responses from a null collection");

        return products.Select(p => FromModel(p));
    }

}
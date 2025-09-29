using Module_01.Models;

namespace Module_01.Interfaces;

public interface IProductRepository
{
    void AddProduct(Product product);
    Task AddProductReviewAsync(ProductReview review, CancellationToken token = default);
    Task DeleteProductAsync(Guid id, CancellationToken token = default);
    Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default);
    Task<bool> ExistsByNameAsync(string? name, CancellationToken token = default);
    Task<Product?> GetProductByIdAsync(Guid productId, CancellationToken token = default);
    Task<List<ProductReview>> GetProductReviewsAsync(Guid productId, CancellationToken token = default);
    Task<int> GetProductsCountAsync(CancellationToken token = default);
    Task<List<Product>> GetProductsPageAsync(int page = 1, int pageSize = 10, CancellationToken token = default);
    Task<ProductReview?> GetReviewAsync(Guid productId, Guid reviewId, CancellationToken token = default);
    Task UpdateProductAsync(Product updatedProduct, CancellationToken token = default);
}

using Microsoft.EntityFrameworkCore;
using Module_01.Models;
using Module_01.Data;
using Module_01.Interfaces;

namespace Module_01.Repositories;

public class EFProductRepository(AppDbContext db) : IProductRepository
{
    public async Task<int> GetProductsCountAsync(CancellationToken token = default) => await db.Products.CountAsync(token);
    public async Task<List<Product>> GetProductsPageAsync(int page = 1, int pageSize = 10, CancellationToken token = default)
    {
        var products = await db.Products.Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync(token);

        return products;
    }
    public async Task<Product?> GetProductByIdAsync(Guid productId, CancellationToken token = default)
    {
        var product = await db.Products.FirstOrDefaultAsync(p => p.Id == productId, token);

        if (product is null)
            return null;

        return product;
    }
    public async Task<List<ProductReview>> GetProductReviewsAsync(Guid productId, CancellationToken token = default)
    {
        return await db.ProductReviews.Where(r => r.ProductId == productId).ToListAsync(token);
    }
    public async Task<ProductReview?> GetReviewAsync(Guid productId, Guid reviewId, CancellationToken token = default)
    {
        return await db.ProductReviews.FirstOrDefaultAsync(r => r.ProductId == productId && r.Id == reviewId, token);
    }
    public void AddProduct(Product product)
    {
        db.Products.Add(product);

    }
    public async Task AddProductReviewAsync(ProductReview review, CancellationToken token = default)
    {
        var product = await db.Products.FirstOrDefaultAsync(p => p.Id == review.ProductId, token);

        if (product is null)
            throw new InvalidOperationException();

        db.ProductReviews.Add(review);

        var reviews = await db.ProductReviews.Where(pr => pr.ProductId == review.ProductId).ToListAsync(token);

        product.AverageRating = (decimal)Math.Round(reviews.Average(pr => pr.Stars), 1, MidpointRounding.AwayFromZero);
    }
    public async Task UpdateProductAsync(Product updatedProduct, CancellationToken token = default)
    {
        var existingProduct = await db.Products.FirstOrDefaultAsync(p => p.Id == updatedProduct.Id, token);

        if (existingProduct == null)
            throw new InvalidOperationException();

        existingProduct.Name = updatedProduct.Name;
        existingProduct.Price = updatedProduct.Price;
    }
    public async Task DeleteProductAsync(Guid id, CancellationToken token = default)
    {
        var product = await db.Products.FirstOrDefaultAsync(p => p.Id == id, token);

        if (product == null)
            throw new InvalidOperationException();

        db.Products.Remove(product);
    }
    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default) => await db.Products.AnyAsync(p => p.Id == id, token);
    public async Task<bool> ExistsByNameAsync(string? name, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        return await db.Products.AnyAsync(p => EF.Functions.Like(p.Name!.ToUpper(), name.ToUpper()), token);
    }
}
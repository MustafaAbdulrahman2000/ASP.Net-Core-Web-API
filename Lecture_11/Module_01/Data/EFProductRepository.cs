using Microsoft.EntityFrameworkCore;
using Module_01.Models;

namespace Module_01.Data;

public class EFProductRepository(AppDbContext db)
{
    public async Task<int> GetProductsCountAsync() => await db.Products.CountAsync();
    public async Task<List<Product>> GetProductsPageAsync(int page = 1, int pageSize = 10)
    {
        var products = await db.Products.Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

        return products;
    }
    public async Task<Product?> GetProductByIdAsync(Guid productId)
    {
        var product = await db.Products.FirstOrDefaultAsync(p => p.Id == productId);

        if (product is null)
            return null;

        return product;
    }
    public async Task<List<ProductReview>> GetProductReviewsAsync(Guid productId)
    {
        return await db.ProductReviews.Where(r => r.ProductId == productId).ToListAsync();
    }
    public async Task<ProductReview?> GetReviewAsync(Guid productId, Guid reviewId)
    {
        return await db.ProductReviews.FirstOrDefaultAsync(r => r.ProductId == productId && r.Id == reviewId);
    }   
    public async Task<bool> AddProductAsync(Product product)
    {
        await db.Products.AddAsync(product);
        return await db.SaveChangesAsync() > 0;
    }
    public async Task<bool> AddProductReviewAsync(ProductReview review)
    {
        if (await db.Products.AnyAsync(p => p.Id == review.ProductId))
            return false;

        await db.ProductReviews.AddAsync(review);

        return await db.SaveChangesAsync() > 0;
    }
    public async Task<bool> UpdateProductAsync(Product updatedProduct)
    {
        var existingProduct = await db.Products.FirstOrDefaultAsync(p => p.Id == updatedProduct.Id);

        if (existingProduct == null)
            return false;

        existingProduct.Name = updatedProduct.Name;
        existingProduct.Price = updatedProduct.Price;

        return await db.SaveChangesAsync() > 0;
    }
    public async Task<bool> DeleteProductAsync(Guid id)
    {
        var product = await db.Products.FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            return false;

        db.Products.Remove(product);
        
        return await db.SaveChangesAsync() > 0;

    }
    public async Task<bool> ExistsByIdAsync(Guid id) => await db.Products.AnyAsync(p => p.Id == id);
    public async Task<bool> ExistsByNameAsync(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        return await db.Products.AnyAsync(p => EF.Functions.Like(p.Name!.ToUpper(), name.ToUpper()));
    }
}
using System.Data;
using Dapper;
using Module_01.Models;

namespace Module_01.Data;

public class DapperProductRepository(IDbConnection db)
{
    public async Task<int> GetProductsCountAsync() =>
        await db.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Products");
    public async Task<List<Product>> GetProductsPageAsync(int page = 1, int pageSize = 10)
    {
        var products = await db.QueryAsync<Product>("SELECT * FROM Products LIMIT @Limit OFFSET @Offset",
            new { Limit = pageSize, Offset = (page - 1) * pageSize });

        return  products.ToList();
    }
    public async Task<Product?> GetProductByIdAsync(Guid productId)
    {
        return await db.QuerySingleOrDefaultAsync<Product>(
            "SELECT * FROM Products WHERE Id = @Id",
            new { Id = productId.ToString() });
    }
    public async Task<List<ProductReview>> GetProductReviewsAsync(Guid productId)
    {
        var products = await db.QueryAsync<ProductReview>(
            "SELECT * FROM ProductReviews WHERE ProductId = @ProductId",
            new { ProductId = productId });
        
        return products.ToList();
    }
    public async Task<ProductReview?> GetReviewAsync(Guid productId, Guid reviewId)
    {
        return await db.QuerySingleOrDefaultAsync<ProductReview>(
            "SELECT * FROM ProductReviews WHERE Id = @Id AND ProductId = @ProductId",
            new { Id = reviewId.ToString(), ProductId = productId.ToString() });
    }   
    public async Task<bool> AddProductAsync(Product product)
    {
        return await db.ExecuteAsync(""" 
        
                        INSERT INTO Products(Id, Name, Price) VALUES
                        (@Id, @Name, @Price);

                        """, 
                        new {Id = product.Id.ToString(), Name = product.Name, Price = product.Price}) > 0;
    }
    public async Task<bool> AddProductReviewAsync(ProductReview review)
    {
        var exists = await db.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Products WHERE Id = @Id",
        new { Id = review.ProductId.ToString() });

        if (exists == 0)
            return false;

        return await db.ExecuteAsync(""" 
        
                        INSERT INTO ProductReviews(Id, ProductId, Reviewer, Stars) VALUES
                        (@Id, @ProductId, @Reviewer, @stars);
                    
                        """,
                        new { Id = review.Id.ToString(), ProductId = review.ProductId.ToString(), Reviewer = review.Reviewer, Stars = review.Stars }) > 0;
    }
    public async Task<bool> UpdateProductAsync(Product updatedProduct)
    {
        return await db.ExecuteAsync(""" 
        
                        UPDATE Products SET Name = @Name, Price = @Price 
                        WHERE Id = @Id;
        
                        """,
                        new 
                        {
                            Name = updatedProduct.Name,
                            Price = updatedProduct.Price,
                            Id = updatedProduct.Id.ToString()
                        }) > 0;
    }
    public async Task<bool> DeleteProductAsync(Guid id)
    {
        var row = await db.ExecuteAsync("DELETE FROM Products WHERE Id = @Id", new { Id = id.ToString() });

        await db.ExecuteAsync("DELETE FROM ProductReviews WHERE ProductId = @ProductId", new { ProductId = id.ToString() });

        return row > 0;
    }
    public async Task<bool> ExistsByIdAsync(Guid id)
    {
        return await db.ExecuteScalarAsync<bool>("SELECT EXISTS(SELECT 1 FROM Products WHERE Id = @Id)", new { Id = id.ToString() });
    }
    public async Task<bool> ExistsByNameAsync(string? name)
    {
           return await db.ExecuteScalarAsync<bool>("SELECT EXISTS(SELECT 1 FROM Products WHERE Name = @Name COLLATE NOCASE)", new { Name = name });
    }
}
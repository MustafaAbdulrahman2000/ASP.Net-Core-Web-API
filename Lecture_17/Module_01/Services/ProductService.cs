using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Module_01.Data;
using Module_01.Models;
using Module_01.Requests;
using Module_01.Responses;


namespace Module_01.Services;

public class ProductService(AppDbContext context) : IProductService
{
    
    /*
    
    public async Task<List<ProductResponse>> GetProductsOldAsync()
    {
        var cacheKey = "products";

        if (cache.TryGetValue(cacheKey, out List<ProductResponse>? products))
        {
            System.Console.WriteLine("Cache visited");

            return products!;
        }

        var entities = await context.Products.ToListAsync();

        products = entities.Select(p => ProductResponse.FromModel(p)).ToList() ?? []; 
        
        cache.Set(cacheKey, products, new MemoryCacheEntryOptions
        {
            Size = 1,
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        });

        return products;
    }
    public async Task<List<ProductResponse>> GetProductsAsync()
    {
        return await cache.GetOrCreate("products", async entry => 
        {
            entry.Size = 1;
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);

            var entities = await context.Products.ToListAsync();

            System.Console.WriteLine("DB visited");

            var response = entities.Select(p => ProductResponse.FromModel(p)).ToList() ?? [];

            return response;
        })!;
    }
    public async Task<ProductResponse?> GetProductByIdAsync(int productId)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == productId);

        if (product is null)
            return null;

        return ProductResponse.FromModel(product);
    }
    public async Task<ProductResponse> AddProductAsync(CreateProductRequest request)
    {
        var product = new Product
        {
            Name = request.Name,
            Price = request.Price
        };

        context.Products.Add(product);

        await context.SaveChangesAsync();

        cache.Remove("products");

        return ProductResponse.FromModel(product);
    }
    public async Task UpdateProductAsync(int productId, UpdateProductRequest request)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == productId);

        if (product is null)
            return;

        product.Name = request.Name;
        product.Price = request.Price;

        await context.SaveChangesAsync();

        cache.Remove("products");
    }
    public async Task DeleteProductAsync(int id)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id);

        if (product is null)
            return;

        context.Products.Remove(product);

        await context.SaveChangesAsync();

        cache.Remove("products");
    }
    
    
    public async Task<List<ProductResponse>> GetProductsAsync()
    {
        var cacheKey = "products";

        var cacheData = await cache.GetStringAsync(cacheKey);

        if (cacheData is not null)
        {
            System.Console.WriteLine("Cache visited");

            return JsonSerializer.Deserialize<List<ProductResponse>>(cacheData)!;
        }

        var entities = await context.Products.ToListAsync();

        var products = entities.Select(p => ProductResponse.FromModel(p)).ToList();

        var jsonData = JsonSerializer.Serialize(products);

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        };

        await cache.SetStringAsync(cacheKey, jsonData, options);
        
        return products;
    }

    public async Task<ProductResponse?> GetProductByIdAsync(int productId)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == productId);

        if (product is null)
            return null;

        return ProductResponse.FromModel(product);
    }
    
    public async Task<ProductResponse> AddProductAsync(CreateProductRequest request)
    {
        var product = new Product
        {
            Name = request.Name,
            Price = request.Price
        };

        context.Products.Add(product);

        await context.SaveChangesAsync();

        await cache.RemoveAsync("products");

        return ProductResponse.FromModel(product);
    }
    
    public async Task UpdateProductAsync(int productId, UpdateProductRequest request)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == productId);

        if (product is null)
            return;

        product.Name = request.Name;
        product.Price = request.Price;

        await context.SaveChangesAsync();        
        
        await cache.RemoveAsync("products");
    }
    
    public async Task DeleteProductAsync(int id)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id);

        if (product is null)
            return;

        context.Products.Remove(product);

        await context.SaveChangesAsync();

        await cache.RemoveAsync("products");
    }

    */
    
    public async Task<PagedResult<ProductResponse>> GetProductsAsync(int page = 1, int pageSize = 10)
    {
        page = Math.Max(1, page);

        pageSize = Math.Clamp(pageSize, 1, 10);

        var entities = await context.Products
            .OrderBy(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var totalCount = await context.Products.CountAsync();

        var products = entities?.Select(p => ProductResponse.FromModel(p)).ToList() ?? [];

        return new PagedResult<ProductResponse>
        {
            CurrentPage = page,
            PageSize = pageSize,
            Items = products,
            TotalCount = totalCount
        };
    }
    public async Task<List<ProductResponse>> GetProductsAsync()
    {
        var entities = await context.Products.ToListAsync();

        return entities.Select(p => ProductResponse.FromModel(p)).ToList();
    }
    public async Task<ProductResponse?> GetProductByIdAsync(int productId)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == productId);

        if (product is null)
            return null;

        return ProductResponse.FromModel(product);
    }
    public async Task<ProductResponse> AddProductAsync(CreateProductRequest request)
    {
        var product = new Product
        {
            Name = request.Name,
            Price = request.Price
        };

        context.Products.Add(product);

        await context.SaveChangesAsync();

        return ProductResponse.FromModel(product);
    }
    public async Task UpdateProductAsync(int productId, UpdateProductRequest request)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == productId);

        if (product is null)
            return;

        product.Name = request.Name;
        product.Price = request.Price;

        await context.SaveChangesAsync();                
    }
    public async Task DeleteProductAsync(int id)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id);

        if (product is null)
            return;

        context.Products.Remove(product);

        await context.SaveChangesAsync();

    }
}

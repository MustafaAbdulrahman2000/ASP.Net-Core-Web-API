using Module_03.Models;
using Module_03.Requests;
using Module_03.Responses;

namespace Module_03.Services;

public interface IProductService
{
    Task<List<ProductResponse>> GetProductsAsync();

    Task<PagedResult<ProductResponse>> GetProductsAsync(int page, int pageSize);
    
    Task<ProductResponse?> GetProductByIdAsync(int productId);
    
    Task<ProductResponse> AddProductAsync(CreateProductRequest request);
    
    Task UpdateProductAsync(int productId, UpdateProductRequest request);
    
    Task DeleteProductAsync(int id);
}

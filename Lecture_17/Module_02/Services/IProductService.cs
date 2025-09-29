using Module_02.Models;
using Module_02.Requests;
using Module_02.Responses;

namespace Module_02.Services;

public interface IProductService
{
    Task<List<ProductResponse>> GetProductsAsync();

    Task<PagedResult<ProductResponse>> GetProductsAsync(int page, int pageSize);
    
    Task<ProductResponse?> GetProductByIdAsync(int productId);
    
    Task<ProductResponse> AddProductAsync(CreateProductRequest request);
    
    Task UpdateProductAsync(int productId, UpdateProductRequest request);
    
    Task DeleteProductAsync(int id);
}

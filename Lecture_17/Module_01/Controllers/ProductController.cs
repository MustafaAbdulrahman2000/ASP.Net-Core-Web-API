using Module_01.Requests;
using Module_01.Responses;
using Module_01.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using System.Text;
using System.Security.Cryptography;
using System.Net.Http.Headers;

namespace Module_01.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService productService) : ControllerBase
{
    /*

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> Get()
    {
        var response = await productService.GetProductsAsync();
        return Ok(response);
    }

     [HttpGet]
    [OutputCache(Duration = 20)]
    public async Task<IActionResult> Get(int page = 1, int pageSize = 10)
    {
        Console.WriteLine("Controller Action visited");

        var PagedResult = await productService.GetProductsAsync(page, pageSize);

        return Ok(PagedResult);
    }

    [HttpGet("{productId:int}", Name = nameof(GetById))]
    [OutputCache(PolicyName = "Single-Product")]
    public async Task<ActionResult<ProductResponse>> GetById(int productId)
    {
        var response = await productService.GetProductByIdAsync(productId);
        if (response is null)
            return NotFound($"Product with Id '{productId}' not found");

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateProductRequest request)
    {
        var response = await productService.AddProductAsync(request);
        
        await cache.EvictByTagAsync("products", default);
        
        return CreatedAtRoute(nameof(GetById), new { productId = response.ProductId }, response);
    }

    [HttpPut("{productId:int}")]
    public async Task<IActionResult> Put(int productId, [FromBody] UpdateProductRequest request)
    {
        await productService.UpdateProductAsync(productId, request);
        
        await cache.EvictByTagAsync("products", default);

        return NoContent();
    }

    [HttpDelete("{productId:int}")]
    public async Task<IActionResult> Delete(int productId)
    {
        await productService.DeleteProductAsync(productId);

        await cache.EvictByTagAsync("products", default);

        return NoContent();
    }

    */

    [HttpGet]
    [ResponseCache(Duration = 20, Location = ResponseCacheLocation.Any, VaryByQueryKeys = ["page"])]
    public async Task<IActionResult> Get(int page = 1, int pageSize = 10)
    {
        Console.WriteLine("Controller Action visited");

        var PagedResult = await productService.GetProductsAsync(page, pageSize);

        return Ok(PagedResult);
    }

    [HttpGet("{productId:int}", Name = nameof(GetById))]
    [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any, VaryByHeader = "If-None-Match")]
    public async Task<ActionResult<ProductResponse>> GetById(int productId)
    {
        var response = await productService.GetProductByIdAsync(productId);
        
        if (response is null)
            return NotFound($"Product with Id '{productId}' not found");

        string etag = GenerateEtag(response);

        if (Request.Headers.IfNoneMatch == etag)
            return StatusCode(StatusCodes.Status304NotModified);

        Response.Headers.IfNoneMatch = new EntityTagHeaderValue(etag).ToString();

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateProductRequest request)
    {
        var response = await productService.AddProductAsync(request);
        
        return CreatedAtRoute(nameof(GetById), new { productId = response.ProductId }, response);
    }

    [HttpPut("{productId:int}")]
    public async Task<IActionResult> Put(int productId, [FromBody] UpdateProductRequest request)
    {
        await productService.UpdateProductAsync(productId, request);
        
        return NoContent();
    }

    [HttpDelete("{productId:int}")]
    public async Task<IActionResult> Delete(int productId)
    {
        await productService.DeleteProductAsync(productId);

        return NoContent();
    }

    private string GenerateEtag(ProductResponse product)
    {
        var raw = $"{product.ProductId}|{product.Name}|{product.Price}";
        var bytes = Encoding.UTF8.GetBytes(raw);
        var hash = SHA256.HashData(bytes);

        return $"\"{Convert.ToBase64String(hash)}\"";
    }
}
   
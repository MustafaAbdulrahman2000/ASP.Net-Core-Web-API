using Module_02.Requests;
using Module_02.Responses;
using Module_02.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using System.Text;
using System.Security.Cryptography;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.RateLimiting;

namespace Module_02.Controllers;

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
    [EnableRateLimiting(policyName: "FixedLimiter")]
    public async Task<IActionResult> Get(int page = 1, int pageSize = 10)
    {
        var PagedResult = await productService.GetProductsAsync(page, pageSize);

        return Ok(PagedResult);
    }

    [HttpGet("{productId:int}", Name = nameof(GetById))]
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
}
   
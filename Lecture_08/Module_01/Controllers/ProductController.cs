using System.Text;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Module_01.Data;
using Module_01.Models;
using Module_01.Requests;
using Module_01.Responses;

namespace Module_01.Controllers;

[ApiController]
[Route("api/product")]
public class ProductController(ProductRepository repository) : ControllerBase
{
    /*

    [HttpGet]
    public string Get()
    {
        return "Product [1], Price: $4.99";
    }

    */

    [HttpOptions]
    public IActionResult OptionsProducts()
    {
        Response.Headers.Append("Allow", "GET, HEAD, POST, PUT, PATCH, DELETE, OPTIONS");

        return NoContent();
    }

    [HttpHead("{productId:guid}")]
    public IActionResult HeadProduct(Guid productId)
    {
        return repository.ExistsById(productId) ? Ok() : NotFound();
    }

    [HttpGet("{productId:guid}", Name = "GetProductById")]
    [Produces("application/json", "application/xml")]
    public ActionResult<ProductResponse> GetProductById(Guid productId, bool includeReviews = false)
    {
        var product = repository.GetProductById(productId);

        if (product is null)
            return NotFound();

        List<ProductReview>? reviews = null;

        if (includeReviews)
            reviews = repository.GetProductReviews(productId);

        return ProductResponse.FromModel(product, reviews!);
    }

    [HttpGet]
    public IActionResult GetPaged(int page = 1, int pageSize = 10)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        int totalCount = repository.GetProductsCount();

        var products = repository.GetProductsPage(page, pageSize);

        var pagedResponse = PagedResponse<ProductResponse>.Create(ProductResponse.FromModels(products), totalCount, page, pageSize);

        return Ok(pagedResponse);
    }

    [HttpPost]
    public IActionResult CreateProduct(CreateProductRequest request)
    {
        if (repository.ExistsByName(request.Name))
            return Conflict($"A product with the name {request.Name} already exists.");

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Price = request.Price
        };

        repository.AddProduct(product);

        return CreatedAtRoute(routeName: nameof(GetProductById),
                              routeValues: new { productId = product.Id },
                              value: ProductResponse.FromModel(product));
    }

    [HttpPut("{productId:guid}")]
    public IActionResult Put(Guid productId, UpdateProductRequest request)
    {
        var product = repository.GetProductById(productId);

        if (product is null)
            return NotFound($"product with Id '{productId}' not found.");

        product.Name = request.Name;
        product.Price = request.Price;

        var succeeded = repository.UpdateProduct(product);

        if (!succeeded)
            return StatusCode(500, "Failed to update product.");

        return NoContent();
    }

    [HttpDelete("{productId:guid}")]
    public IActionResult Delete(Guid productId)
    {
        if (!repository.ExistsById(productId))
            return NotFound($"Product with Id '{productId}' not found.");

        var succeeded = repository.DeleteProduct(productId);

        if (!succeeded)
            return StatusCode(500, "Failed to delete product.");

        return NoContent();
    }

    [HttpPatch("{productId:guid}")]
    public IActionResult Patch(Guid productId, JsonPatchDocument<UpdateProductRequest> patchDoc)
    {
        if (patchDoc is null)
            return BadRequest($"Invalid patch document.");

        var product = repository.GetProductById(productId);

        if (product is null)
            return NotFound($"Product with Id '{productId}' not found.");

        var updateModel = new UpdateProductRequest
        {
            Name = product.Name,
            Price = product.Price
        };

        patchDoc.ApplyTo(updateModel);
        
        product.Name = updateModel.Name;
        product.Price = updateModel.Price;

        var succeeded = repository.UpdateProduct(product);

        if (!succeeded)
            return StatusCode(500, "Failed to patch product.");

        return NoContent();
    }

    [HttpPost("process")]
    public IActionResult ProcessAsync()
    {
        var jobId = Guid.NewGuid();

        return Accepted(
            $"api/product/status/{jobId}",
            new { jobId, Status = "Processing"}
        );
    }

    [HttpGet("status/{jobId}")]
    public IActionResult GetProcessingStatus(Guid jobId)
    {
        var isStillProcessing = false;

        return Ok(new { jobId, Status = isStillProcessing ? "Processing" : "Completed" });
    }

    [HttpGet("csv")]
    public IActionResult GetProductCSV()
    {
        var products = repository.GetProductsPage(1, 100);

        var csvBuilder = new StringBuilder();

        csvBuilder.AppendLine("Id,Name,Price");

        foreach (var product in products)
        {
            csvBuilder.AppendLine($"{product.Id},{product.Name},{product.Price}");
        }

        var fileBytes = Encoding.UTF8.GetBytes(csvBuilder.ToString());

        return File(fileBytes, "text/csv", "product-catalog.csv");
    }

    [HttpGet("physical-csv-file")]
    public IActionResult GetPhysicalFile()
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", "products.csv");

        return PhysicalFile(filePath, "text/csv", "products-export.csv");
    }

    [HttpGet("products-legacy")]
    public IActionResult GetRedirect()
    {
        return Redirect("/api/product/temp-products");
    }

    [HttpGet("temp-products")]
    public IActionResult TempProducts()
    {
        return Ok(new { message = "you 're in the temp endpoint. chill"});
    }

    [HttpGet("legacy-products")]
    public IActionResult GetPermanentRedirect()
    {
        return RedirectPermanent("/api/product/product-catalog");
    }

    [HttpGet("product-catalog")]
    public IActionResult Catalog()
    {
        return Ok(new { message = "This is the permanent new location."});
    }

    [HttpGet("products-table")]
    [Produces("text/primitives-table")]
    public IActionResult GetProductsAsTable()
    {
        var products = repository.GetProductsPage(1, 100);

        return Ok(products);
    }
}
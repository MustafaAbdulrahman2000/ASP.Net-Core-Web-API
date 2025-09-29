using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Module_01.Data;
using Module_01.Models;
using Module_01.Requests;
using Module_01.Responses;
using Newtonsoft.Json;

namespace Module_01.MinimalEndpoints;

public static class ProductEndpoints
{
    public static RouteGroupBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var productApi = app.MapGroup("/api/products");

        productApi.MapMethods("", ["OPTIONS"], OptionsProducts);
        productApi.MapMethods("{productId:guid}", ["HEAD"], HeadProduct);
        productApi.MapGet("", GetPaged);
        productApi.MapGet("{productId:guid}", GetProductById).WithName(nameof(GetProductById));
        productApi.MapPost("", CreateProduct);
        productApi.MapPut("{productId:guid}", Put);
        productApi.MapDelete("{productId:guid}", Delete);
        productApi.MapPatch("{productId:guid}", Patch);
        productApi.MapPost("process", ProcessAsync);
        productApi.MapGet("status/{jobId}", GetProcessingStatus);
        productApi.MapGet("csv", GetProductCSV);
        productApi.MapGet("physical-csv-file", GetPhysicalFile);
        productApi.MapGet("products-legacy", GetRedirect);
        productApi.MapGet("temp-products", TempProducts);
        productApi.MapGet("legacy-products", GetPermanentRedirect);
        productApi.MapGet("product-catalog", Catalog);

        return productApi;
    }

    private static IResult OptionsProducts(HttpResponse response)
    {
        response.Headers.Append("Allow", "GET, HEAD, POST, PUT, PATCH, DELETE, OPTIONS");

        return Results.NoContent();
    }
    private static IResult HeadProduct(Guid productId, ProductRepository repository)
    {
        return repository.ExistsById(productId) ? Results.Ok() : Results.NotFound();
    }
    private static Results<Ok<ProductResponse>, NotFound> GetProductById(ProductRepository repository, Guid productId, bool includeReviews = false)
    {
        var product = repository.GetProductById(productId);

        if (product is null)
            return TypedResults.NotFound();

        List<ProductReview>? reviews = null;

        if (includeReviews)
            reviews = repository.GetProductReviews(productId);

        return TypedResults.Ok(ProductResponse.FromModel(product, reviews!));
    }
    private static IResult GetPaged(ProductRepository repository, int page = 1, int pageSize = 10)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        int totalCount = repository.GetProductsCount();

        var products = repository.GetProductsPage(page, pageSize);

        var pagedResponse = PagedResponse<ProductResponse>.Create(ProductResponse.FromModels(products), totalCount, page, pageSize);

        return Results.Ok(pagedResponse);
    }
    private static IResult CreateProduct(CreateProductRequest request, ProductRepository repository)
    {
        if (repository.ExistsByName(request.Name))
            return Results.Conflict($"A product with the name {request.Name} already exists.");

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Price = request.Price
        };

        repository.AddProduct(product);

        return Results.CreatedAtRoute(routeName: nameof(GetProductById),
                              routeValues: new { productId = product.Id },
                              value: ProductResponse.FromModel(product));
    }
    private static IResult Put(Guid productId, UpdateProductRequest request, ProductRepository repository)
    {
        var product = repository.GetProductById(productId);

        if (product is null)
            return Results.NotFound($"product with Id '{productId}' not found.");

        product.Name = request.Name;
        product.Price = request.Price;

        var succeeded = repository.UpdateProduct(product);

        if (!succeeded)
            return Results.StatusCode(500);

        return Results.NoContent();
    }
    private static IResult Delete(Guid productId, ProductRepository repository)
    {
        if (!repository.ExistsById(productId))
            return Results.NotFound($"Product with Id '{productId}' not found.");

        var succeeded = repository.DeleteProduct(productId);

        if (!succeeded)
            return Results.StatusCode(500);

        return Results.NoContent();
    }
    private static async Task<IResult> Patch(Guid productId, ProductRepository repository, HttpRequest request)
    {
        using var reader = new StreamReader(request.Body);

        var json = await reader.ReadToEndAsync();

        var patchDoc = JsonConvert.DeserializeObject<JsonPatchDocument<UpdateProductRequest>>(json);

        if (patchDoc is null)
            return Results.BadRequest($"Invalid patch document.");

        var product = repository.GetProductById(productId);

        if (product is null)
            return Results.NotFound($"Product with Id '{productId}' not found.");

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
            return Results.StatusCode(500);

        return Results.NoContent();
    }
    private static IResult ProcessAsync()
    {
        var jobId = Guid.NewGuid();

        return Results.Accepted(
            $"api/product/status/{jobId}",
            new { jobId, Status = "Processing"}
        );
    }
    private static IResult GetProcessingStatus(Guid jobId)
    {
        var isStillProcessing = false;

        return Results.Ok(new { jobId, Status = isStillProcessing ? "Processing" : "Completed" });
    }
    private static IResult GetProductCSV(ProductRepository repository)
    {
        var products = repository.GetProductsPage(1, 100);

        var csvBuilder = new StringBuilder();

        csvBuilder.AppendLine("Id,Name,Price");

        foreach (var product in products)
        {
            csvBuilder.AppendLine($"{product.Id},{product.Name},{product.Price}");
        }

        var fileBytes = Encoding.UTF8.GetBytes(csvBuilder.ToString());

        return Results.File(fileBytes, "text/csv", "product-catalog.csv");
    }
    private static IResult GetPhysicalFile()
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", "products.csv");

        return TypedResults.PhysicalFile(filePath, "text/csv", "products-export.csv");
    }
    private static IResult GetRedirect()
    {
        return Results.Redirect("/api/product/temp-products");
    }
    private static IResult TempProducts()
    {
        return Results.Ok(new { message = "you 're in the temp endpoint. chill"});
    }
    private static IResult GetPermanentRedirect()
    {
        return Results.Redirect("/api/product/product-catalog", permanent: true);
    }
    private static IResult Catalog()
    {
        return Results.Ok(new { message = "This is the permanent new location."});
    }
}
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Module_02.Responses;
using Module_02.Requests;
using Module_02.Services;

namespace Module_02.Endpoints;

public static class MinimalEndpoints
{
    public static RouteGroupBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var productsApi = app.MapGroup("/api/products-minimal");

        productsApi.MapGet("", GetProducts);

        productsApi.MapGet("/paged", GetPagedProducts)
                   .RequireRateLimiting(policyName: "FixedLimiter");

        productsApi.MapGet("/{productId:int}", GetProductById);
        productsApi.MapPost("", CreateProduct);
        productsApi.MapPut("/{productId:int}", UpdateProduct);
        productsApi.MapDelete("/{productId:int}", DeleteProduct);

        return productsApi;
    }

    private static async Task<Ok<List<ProductResponse>>> GetProducts(IProductService service)
    {
        var response = await service.GetProductsAsync();

        return TypedResults.Ok(response);
    }
    private static async Task<Ok<PagedResult<ProductResponse>>> GetPagedProducts(IProductService service, int page = 1, int pageSize = 10)
    {
        Console.WriteLine("Minimal endpoint Action visited");

        var PagedResult = await service.GetProductsAsync(page, pageSize);

        return TypedResults.Ok(PagedResult);
    }
    private static async Task<Results<NotFound<string>, Ok<ProductResponse>>> GetProductById(int productId, IProductService service)
    {
        var response = await service.GetProductByIdAsync(productId);

        if (response is null)
            return TypedResults.NotFound($"Product with Id '{productId}' not found");

        return TypedResults.Ok(response);
    }
    private static async Task<IResult> CreateProduct([FromBody] CreateProductRequest request, IProductService service)
    {
        var response = await service.AddProductAsync(request);

        return Results.Created($"api/products-minimal/{response.ProductId}",response);
    }
    private static async Task<IResult> UpdateProduct(int productId, [FromBody] UpdateProductRequest request, IProductService service)
    {
        await service.UpdateProductAsync(productId, request);

        return Results.NoContent();
    }
    private static async Task<IResult> DeleteProduct(int productId, IProductService service)
    {
        await service.DeleteProductAsync(productId);

        return Results.NoContent();
    }
}

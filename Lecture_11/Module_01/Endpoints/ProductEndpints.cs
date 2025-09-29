using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Module_01.Interfaces;
using Module_01.Models;
using Module_01.Requests;
using Module_01.Responses;

namespace Module_01.Endpoints;

public static class ProductEndpoints
{
    public static RouteGroupBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var productApi = app.MapGroup("/api/products");

        productApi.MapGet("", GetPaged);
        productApi.MapGet("{productId:guid}", GetProductById).WithName(nameof(GetProductById));
        productApi.MapPost("", CreateProduct);
        productApi.MapPut("{productId:guid}", Put);
        productApi.MapDelete("{productId:guid}", Delete);

        return productApi;
    }

    private static async Task<IResult> GetProductById(IUnitOfWork unitOfWork, Guid productId, bool includeReviews = false, CancellationToken token = default)
    {
        var product = await unitOfWork.Products.GetProductByIdAsync(productId, token);

        if (product is null)
            return TypedResults.NotFound();

        List<ProductReview>? reviews = null;

        if (includeReviews)
            reviews = await unitOfWork.Products.GetProductReviewsAsync(productId, token);

        return TypedResults.Ok(ProductResponse.FromModel(product, reviews!));
    }
    private static async Task<IResult> GetPaged(IUnitOfWork unitOfWork, int page = 1, int pageSize = 10, CancellationToken token = default)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        int totalCount = await unitOfWork.Products.GetProductsCountAsync(token);

        var products = await unitOfWork.Products.GetProductsPageAsync(page, pageSize, token);

        var pagedResponse = PagedResponse<ProductResponse>.Create(ProductResponse.FromModels(products), totalCount, page, pageSize);

        return Results.Ok(pagedResponse);
    }
    private static async Task<IResult> CreateProduct(CreateProductRequest request, IUnitOfWork unitOfWork, CancellationToken token = default)
    {
        if (await unitOfWork.Products.ExistsByNameAsync(request.Name, token))
            return Results.Conflict($"A product with the name {request.Name} already exists.");

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Price = request.Price
        };

        unitOfWork.Products.AddProduct(product);
        await unitOfWork.SaveChangesAsync(token);

        return Results.CreatedAtRoute(routeName: nameof(GetProductById),
                              routeValues: new { productId = product.Id },
                              value: ProductResponse.FromModel(product));
    }
    private static async Task<IResult> Put(Guid productId, UpdateProductRequest request, IUnitOfWork unitOfWork, CancellationToken token = default)
    {
        var product = await unitOfWork.Products.GetProductByIdAsync(productId, token);

        if (product is null)
            return Results.NotFound($"product with Id '{productId}' not found.");

        product.Name = request.Name;
        product.Price = request.Price;

        await unitOfWork.Products.UpdateProductAsync(product, token);
        await unitOfWork.SaveChangesAsync(token);

        /*
        
            if (!succeeded)
                return Results.StatusCode(500);
        
        */

        return Results.NoContent();
    }
    private static async Task<IResult> Delete(Guid productId, IUnitOfWork unitOfWork, CancellationToken token = default)
    {
        var found = await unitOfWork.Products.ExistsByIdAsync(productId, token);
        
        if (!found)
            return Results.NotFound($"Product with Id '{productId}' not found.");

        await unitOfWork.Products.DeleteProductAsync(productId, token);
        await unitOfWork.SaveChangesAsync(token);
        
        /*

            if (!succeeded)
                return Results.StatusCode(500);
        
        */
       
        return Results.NoContent();
    }
}
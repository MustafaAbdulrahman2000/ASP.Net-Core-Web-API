using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Module_02.Data;
using Module_02.Responses.V2;
using Asp.Versioning;

namespace Module_01.Endpoints.V2;

public static class ProductEndpoints
{
    public static RouteGroupBuilder MapProductEndpointsV2(this IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {

        /*
            var productApi = app.MapGroup("/api/v{version:apiVersion}/products")
                            .WithApiVersionSet(apiVersionSet);

        */

        var productApi = app.MapGroup("/api/products")
                            .WithApiVersionSet(apiVersionSet);

        productApi.MapGet("/{id}", GetProductById)
                  .HasApiVersion(new ApiVersion(2, 0))
                  .WithName("GetProductByIdV2");

        return productApi;
    }

    private static Results<Ok<ProductResponse>, NotFound<string>> GetProductById(Guid id, ProductRepository repository)
    {
        var product = repository.GetProductById(id);

        if (product is null)
            return TypedResults.NotFound($"Product with Id '{id}' not found");

        return TypedResults.Ok(ProductResponse.FromModel(product));
    }
}
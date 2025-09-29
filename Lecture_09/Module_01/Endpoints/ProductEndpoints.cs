namespace Module_01.Endpoints;

public static class ProductEndpoints
{
    public static RouteGroupBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var productApi = app.MapGroup("/api/products");

        productApi.MapGet("/", GetAllProducts);

        productApi.MapGet("/{id}", GetProductById);

        productApi.MapPost("/", CreateProduct);

        productApi.MapPut("/{id}", UpdateProduct);

        productApi.MapPatch("/{id}", PatchProduct);

        productApi.MapDelete("/{id}", DeleteProduct);

        return productApi;
    }

    static IResult GetAllProducts() => Results.Ok();
    static IResult GetProductById(Guid id) => Results.Ok();
    static IResult CreateProduct() => Results.Created();
    static IResult UpdateProduct() => Results.NoContent();
    static IResult PatchProduct() => Results.NoContent();
    static IResult DeleteProduct() => Results.NoContent();
}
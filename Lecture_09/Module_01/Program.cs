using Microsoft.AspNetCore.Http.HttpResults;
using Module_01.Data;
using Module_01.MinimalEndpoints;
using Module_01.Models;
using Module_01.Responses;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ProductRepository>();

var app = builder.Build();

/*

app.MapGet("/api/products",() => Results.Ok());

app.MapPost("/api/products",() => Results.Ok());

app.MapPut("/api/products/{id}",(Guid id) => Results.NoContent());

app.MapPatch("/api/products/{id}",(Guid id) => Results.Ok());

app.MapDelete("/api/products/{id}",(Guid id) => Results.Ok());

app.MapMethods("/api/products", ["OPTIONS"], () => Results.NoContent());

app.MapMethods("/api/products", ["HEAD"], () => Results.NoContent());

app.MapGet("/api/products", (ProductRepository repository) => {
    // Handles retrieving a list of products.
    return Results.Ok(repository.GetProductsPage());
});

app.MapGet("api/products/{id}", (Guid id, ProductRepository repository) => {
    // Handles retrieving a single product by its unique identifier.
    var product = repository.GetProductById(id);

    return product is null ? 
           Results.NotFound() :
           Results.Ok(ProductResponse.FromModel(product));
});

app.MapGet("/api/products", GetProducts);

app.MapGet("/api/products/{id:guid}", GetProductById);

IResult GetProducts(ProductRepository repository)
{
    return Results.Ok(repository.GetProductsPage());
}

IResult GetProductById(Guid id, ProductRepository repository)
{
    var product = repository.GetProductById(id);

    return product is null ? 
           Results.NotFound() :
           Results.Ok(ProductResponse.FromModel(product));
}

app.MapGet("/text", () => "Hello world");

app.MapGet("/json", () => new { Message = "Hello" });

app.MapGet("/api/products-le-ir/{id:guid}", (Guid id, ProductRepository repository) => {
    
    var product = repository.GetProductById(id);

    return product is null ?
           Results.NotFound() :
           Results.Ok(product);
});

app.MapGet("/api/products-le-tr/{id:guid}", Results<Ok<Product>, NotFound>(Guid id, ProductRepository repository) => {
    
    var product = repository.GetProductById(id);

    return product is null ?
           TypedResults.NotFound() :
           TypedResults.Ok(product);
});

app.MapGet("/api/products-mr-ir/{id:guid}", GetProductByIResult);

app.MapGet("/api/products-mr-tr/{id:guid}", GetProductByTypedResult);

static IResult GetProductByIResult(Guid id, ProductRepository repository)
{
    var product = repository.GetProductById(id);

    return product is null ?
           Results.NotFound() :
           Results.Ok(product);
}

static Results<Ok<Product>, NotFound> GetProductByTypedResult(Guid id, ProductRepository repository)
{
    var product = repository.GetProductById(id);

    return product is null ?
           TypedResults.NotFound() :
           TypedResults.Ok(product);
}

*/

app.MapProductEndpoints();

app.Run();
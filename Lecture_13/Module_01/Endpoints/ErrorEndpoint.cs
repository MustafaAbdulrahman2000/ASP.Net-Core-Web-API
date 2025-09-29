using System.ComponentModel.DataAnnotations;

namespace Module_01.Endpoints;

public static class ErrorEndpoint
{
    public static RouteGroupBuilder MapErrorEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/minimal-fake-errors");

        group.MapGet("/server-error", () => 
        {
            System.IO.File.ReadAllText("E:\\something.json");

            return Results.Ok();
        });

        group.MapPost("/bad-request", () => Results.Problem
        (
            type: "https://example.com/probs/sku-required",
            title: "Bad Request",
            detail: "Product SKU is required.",
            statusCode: StatusCodes.Status400BadRequest
        ));
        
        group.MapPost("/bad-request-no-body", () => Results.BadRequest());

        group.MapPost("/not-found", () => Results.Problem
        (
            type: "https://example.com/probs/product-not-found",
            title: "Not Found",
            detail: "Product not found.",
            statusCode: StatusCodes.Status404NotFound
        ));
        
        group.MapPost("/unauthorized", () => Results.Unauthorized());

        group.MapPost("/conflict", () => Results.Problem
        (   
            type: "https://example.com/probs/product-existed",
            title: "Conflict",
            detail: "This product already exists.",
            statusCode: StatusCodes.Status409Conflict
        ));
        
        group.MapPost("/business-rule-error", () => { throw new ValidationException("A discontinued product can not be put on promotion."); });

        return group;
    }
}

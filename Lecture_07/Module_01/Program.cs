using Microsoft.AspNetCore.Mvc;
using Module_01.Models;
using Module_01.Requests;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
                .AddXmlSerializerFormatters();

var app = builder.Build();

/*

app.MapGet("/product-minimal/{id:int}", (int id) => {
    return Results.Ok(id);
});

app.MapGet("/product-minimal-1/{id:int}", ([FromRoute(Name = "id")] int identifier) => {
    return Results.Ok(identifier);
});

app.MapGet("/product-minimal-2/{id:int}", ([FromQuery] int id) => {
    return Results.Ok(id);
});

app.MapGet("/product-minimal", (int page, int pageSize) => {
    return Results.Ok($"Showing {pageSize} items of page # {page}");
});

app.MapGet("/product-minimal-1", ([FromQuery(Name = "page")] int p, [FromQuery(Name = "pageSize")] int ps) => {
    return  Results.Ok($"Showing {ps} items of page # {p}");
});

app.MapGet("/product-minimal-asparameters", ([AsParameters] SearchRequest request) => {
    return  Results.Ok(request);
});

app.MapGet("/product-minimal-array", (Guid[] ids) => {
    return  Results.Ok(ids);
});

app.MapGet("/date-range-minimal", (DateRangeQuery dateRange) => {
    return  Results.Ok(dateRange);
});

app.MapGet("/product-minimal", ([FromHeader(Name = "X-Api-Version")] string apiVersion) => {
    return Results.Ok($"API version : {apiVersion}");
});


public class SearchRequest
{
    public string? Query { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}

app.MapGet("/product-minimal", ([FromForm] string name, [FromForm] decimal price) => {
    return Results.Ok(new { name, price});
}).DisableAntiforgery();


app.MapPost("/upload-minimal", async (IFormFile file) => { 
    if(file is null || file.Length == 0)
        return Results.BadRequest("No file uploaded.");

    var uplaods = Path.Combine(Directory.GetCurrentDirectory(), "uplaods");
    
    Directory.CreateDirectory(uplaods);
    
    var path = Path.Combine(uplaods, file.FileName);
    
    using var stream = new FileStream(path, FileMode.Create);

    await file.CopyToAsync(stream);

    return Results.Ok("Uploaded");
}).DisableAntiforgery();

app.MapPost("/product-minimal", (ProductRequest request) => {
    return Results.Ok(request);
});

*/

app.MapGet("/mn/preferences", (HttpContext context) => {
    var theme = context.Request.Cookies["theme"];
    var language = context.Request.Cookies["language"];
    var timezone = context.Request.Cookies["timeZone"];

    return Results.Ok(new {
        Theme = theme,
        Language = language,
        Timezone = timezone
    });
});

app.MapControllers();

app.Run();


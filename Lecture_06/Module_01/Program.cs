using System.Diagnostics.Contracts;
using Module_01.Constraints;
using Module_01.Transformers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(opt => opt.ConstraintMap["slugify"] = typeof(SlugifyTransformer));

/*

builder.Services.AddControllers();
builder.Services.AddRouting(opt => opt.ConstraintMap.Add("validMonth",typeof(MonthRouteConstraint)));

*/

var app = builder.Build();

/*

app.MapControllers();

app.MapGet("/products", () =>
{
    return Results.Ok(new[] {
        "Product [1]",
        "Product [2]"
    });
});

app.MapGet("/route-table", (IServiceProvider sp) =>
{
    var endpoints = sp.GetRequiredService<EndpointDataSource>()
                      .Endpoints.Select(ep => ep.DisplayName);

    return Results.Ok(endpoints);
});

app.UseRouting();

app.UseEndpoints(ep => 
{
    ep.MapControllers();
    ep.MapGet("/products", () =>
    {
        return Results.Ok(new[] {
            "Product [1]",
            "Product [2]"
        });
    });
});


app.Use(async (context, next) => {
    var endpoint = context.GetEndpoint()?.DisplayName ?? "No endpoint defined !!";
    
    System.Console.WriteLine($"Middleware [1] : {endpoint}");

    await next();
});

app.Use(async (context, next) => {
    var endpoint = context.GetEndpoint()?.DisplayName ?? "No endpoint defined !!";
    
    System.Console.WriteLine($"Middleware [2] : {endpoint}");

    await next();
});

app.MapGet("/products", () =>{
    return Results.Ok(new[] {
        "Product [1]",
        "Product [2]"
    });
});

app.MapGet("/product/{id}", (int id) => $"Product [{id}]");

app.MapGet("/date/{year}-{month}-{day}", (int year, int month, int day)
    => $"Date is {new DateOnly(year, month, day)}");

app.MapGet("/{controller=Home}", (string? controller) => controller);

app.MapGet("/user/{id?}", (int? id) => id is null ? "All users": $"User {id}");

app.MapGet("/a{b}c{d}", (string b, string d) => $"b: {b}, d: {d}");

app.MapGet("/single/{*slug}", (string slug) => $"Slug: {slug}");

app.MapGet("/double/{**slug}", (string slug) => $"Slug: {slug}");

app.MapGet("/int/{id:int}", (int id) => $"Integer: {id}");

app.MapGet("/bool/{active:bool}", (bool active) => $"Boolean: {active}");

app.MapGet("/datetime/{dob:datetime}", (DateTime dob) => $"DateTime: {dob}");

app.MapGet("/decimal/{price:decimal}", (decimal price) => $"Decimal: {price}");

app.MapGet("/double/{weight:double}", (double weight) => $"Double: {weight}");

app.MapGet("/float/{weight:float}", (float weight) => $"Float: {weight}");

app.MapGet("/guid/{id:guid}", (Guid id) => $"GUID: {id}");

app.MapGet("/long/{ticks:long}", (long ticks) => $"Long: {ticks}");

app.MapGet("/minlength/{filename:minlength(4)}", (string filename) => $"Min Length (4) : {filename}");

app.MapGet("/maxlength/{filename:maxlength(8)}", (string filename) => $"Min Length (8) : {filename}");

app.MapGet("/length/{filename:length(12)}", (string filename) => $"Exact Length (12) : {filename}");

app.MapGet("/lengthrange/{filename:length(8,16)}", (string filename) => $"Length (8, 16) : {filename}");

app.MapGet("/min/{age:min(18)}", (int age) => $"Min Age (18) : {age}");

app.MapGet("/max/{age:max(120)}", (int age) => $"Max Age (120) : {age}");

app.MapGet("/range/{age:range(18,120)}", (int age) => $"Range (18, 120) : {age}");

app.MapGet("/alpha/{name:alpha}", (string name) => $"Alpha: {name}");

app.MapGet("/regex/{ssn:regex(^\\d{{3}}-\\d{{2}}-\\d{{4}}$)}", (string ssn) => $"Regex Match (SSN): {ssn}");

app.MapGet("/required/{name:required}", (string required) => $"Required: {required}");

app.MapGet("/custom/{month:validMonth}", (int month) => $"Month: {month}");

app.MapGet("/order/{id:int}", (int id, LinkGenerator link, HttpContext context) => {
    // order is retrieved

    var editUrl = link.GetUriByName
    (
        "EditOrder",
        new { id },
        context.Request.Scheme,
        context.Request.Host
    );

    return Results.Ok(new {
        orderId = id,
        status = "PENDING",
        _links = new {
            self = new { href = context.Request.Path },
            edit = new { href = editUrl, Method = "Put" }
        }
    });
});

app.MapPut("/order/{id:int}", (int id) => {
    // order is updated

    return Results.NoContent();
}).WithName("EditOrder");

*/

app.MapGet("/Details/{name:slugify}", (string name) => {
  
    return Results.Ok(new { name });
}).WithName("DetailsByName");

app.MapGet("/generate", (LinkGenerator link, HttpContext context) => {
    var url = link.GetPathByName("DetailsByName", new { name = "Mustafa Ahmed Abdulrahman" });
    
    return Results.Ok(new { generatedUrl = url });
});

app.Run();

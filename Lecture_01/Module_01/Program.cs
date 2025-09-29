var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/authors/{author}", async (string author, HttpContext context) => {
    var data = new 
    {
        Id = context.TraceIdentifier,
        Scheme = context.Request.Scheme,
        Method = context.Request.Method,
        Host = context.Request.Host,
        Path = context.Request.Path,
        Query = context.Request.Query,
        Headers = context.Request.Headers,
        RouteValues = context.Request.RouteValues,
        Body = await new StreamReader(context.Request.Body).ReadToEndAsync()
    };

    return Results.Ok(data);
});

app.Run();

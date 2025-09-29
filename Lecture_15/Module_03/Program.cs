var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:7070")
              .AllowAnyMethod();
    });
});

/*

builder.Services.AddHttpsRedirection(opt =>
{
    opt.RedirectStatusCode = StatusCodes.Status301MovedPermanently;
});

*/

var app = builder.Build();

app.UseCors();

app.MapGet("/api/bookings", () =>
{
    return Results.Ok(new List<object>
    {
        new { Id = Guid.NewGuid(), Room = "A100", CustomerName = "Alice", CheckIn = new DateTime(2025, 6, 1), CheckOut = new DateTime(2025, 6, 5) },
        new { Id = Guid.NewGuid(), Room = "A101", CustomerName = "Bob", CheckIn = new DateTime(2025, 6, 3), CheckOut = new DateTime(2025, 6, 7) },
        new { Id = Guid.NewGuid(), Room = "A102", CustomerName = "Charlie", CheckIn = new DateTime(2025, 6, 10), CheckOut = new DateTime(2025, 6, 12) },
        new { Id = Guid.NewGuid(), Room = "A103", CustomerName = "Diana", CheckIn = new DateTime(2025, 6, 8), CheckOut = new DateTime(2025, 6, 9) },
        new { Id = Guid.NewGuid(), Room = "A104", CustomerName = "Ethan", CheckIn = new DateTime(2025, 6, 15), CheckOut = new DateTime(2025, 6, 18) },
        new { Id = Guid.NewGuid(), Room = "A105", CustomerName = "Fiona", CheckIn = new DateTime(2025, 6, 20), CheckOut = new DateTime(2025, 6, 22) },
        new { Id = Guid.NewGuid(), Room = "A106", CustomerName = "George", CheckIn = new DateTime(2025, 6, 25), CheckOut = new DateTime(2025, 6, 28) },
        new { Id = Guid.NewGuid(), Room = "A107", CustomerName = "Hannah", CheckIn = new DateTime(2025, 6, 11), CheckOut = new DateTime(2025, 6, 13) },
        new { Id = Guid.NewGuid(), Room = "A108", CustomerName = "Ian", CheckIn = new DateTime(2025, 6, 5), CheckOut = new DateTime(2025, 6, 6) },
        new { Id = Guid.NewGuid(), Room = "A109", CustomerName = "Judy", CheckIn = new DateTime(2025, 6, 29), CheckOut = new DateTime(2025, 6, 30) },
    });
});

/*

app.UseHttpsRedirection();

*/

app.Run();
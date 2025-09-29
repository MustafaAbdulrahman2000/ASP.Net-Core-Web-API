using System.Text.Json.Serialization;
using OrderServiceAPI.Repositories;
using OrderServiceAPI.Services;
using M01.RepositoryPattern.Data;

using Microsoft.EntityFrameworkCore;
using OrderServiceApi.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = (context) =>
    {
        context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
        context.ProblemDetails.Extensions.Add("requestId", context.HttpContext.TraceIdentifier);
    };
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source = app.db");
});

builder.Services.AddHttpClient<IOrderService, OrderService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["PaymentService:BaseUrl"]!);
});

var app = builder.Build();

app.UseExceptionHandler();

app.UseStatusCodePages();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

app.Run();
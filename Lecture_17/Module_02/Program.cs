using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;
using Module_02.Data;
using Module_02.Endpoints;
using Module_02.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source = app.db"));

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("FixedLimiter", limiter =>
    {
        limiter.Window = TimeSpan.FromMinutes(1);
        limiter.PermitLimit = 100;
        limiter.QueueLimit = 10;
        limiter.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

    options.AddSlidingWindowLimiter("SlidingLimiter", limiter =>
    {
        limiter.Window = TimeSpan.FromMinutes(1);
        limiter.PermitLimit = 100;
        limiter.QueueLimit = 10;
        limiter.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiter.SegmentsPerWindow = 6;
        limiter.AutoReplenishment = true;
    });

    options.AddConcurrencyLimiter("ConcurrencyLimiter", limiter =>
    {
        limiter.PermitLimit = 50;
        limiter.QueueLimit = 100;
        limiter.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

    options.AddPolicy("ApiUserPolicy", httpContext =>
            RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.User.Identity?.Name ?? "anonymous",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                Window = TimeSpan.FromMinutes(1),
                PermitLimit = 1000,
                AutoReplenishment = true
            }));

    options.AddPolicy("IpPolicy", httpContext =>
            RateLimitPartition.GetSlidingWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown",
            factory: _ => new SlidingWindowRateLimiterOptions
            {
                Window = TimeSpan.FromMinutes(1),
                PermitLimit = 100,
                SegmentsPerWindow = 6,
                AutoReplenishment = true
            }));
});

var app = builder.Build();

app.UseRateLimiter();
app.MapControllers();
app.MapProductEndpoints();

app.Run();

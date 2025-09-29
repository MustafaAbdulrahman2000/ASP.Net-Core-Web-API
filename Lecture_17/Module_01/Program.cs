using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Microsoft.Extensions.Caching.Hybrid;
using Module_01.Data;
using Module_01.Endpoints;
using Module_01.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddResponseCaching();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source = app.db"));

/*

builder.Services.AddMemoryCache(options => options.SizeLimit = 100);

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "Module_01";
    options.ConfigurationOptions = new ConfigurationOptions
    {
        EndPoints = { "localhost:6379" },
        SyncTimeout = 100_000
    };
});

builder.Services.AddDistributedSqlServerCache(options =>
{
    options.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.SchemaName = "dbo";
    options.TableName = "CacheEntries";
});

builder.Services.AddHybridCache(options => 
{
    options.DefaultEntryOptions = new HybridCacheEntryOptions
    {
        Expiration = TimeSpan.FromMinutes(10),
        LocalCacheExpiration = TimeSpan.FromMinutes(30)
    };
});


builder.Services.AddOutputCache(options => 
{
    options.DefaultExpirationTimeSpan = TimeSpan.FromMinutes(10);
    options.MaximumBodySize = 64 * 1024;
    options.SizeLimit = 100 * 1024 * 1024;
});

builder.Services.AddOutputCache(options => 
{
    options.AddPolicy("Single-Product", policy =>
    {
        policy.Expire(TimeSpan.FromSeconds(20))
              .SetVaryByRouteValue(["productId"]);
        policy.Tag(["products"]);
    });
});

*/

var app = builder.Build();

app.UseResponseCaching();
app.MapProductEndpoints();
app.MapControllers();

/*

app.UseOutputCache();

*/

app.Run();


using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;
using Module_03.Data;
using Module_03.Endpoints;
using Module_03.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<GzipCompressionProvider>();
    options.Providers.Add<BrotliCompressionProvider>();
    options.MimeTypes = 
    [
        "application/json",
        "application/xml",
        "text/plain",
        "text/html"
    ];
});

builder.Services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);
builder.Services.Configure<BrotliCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source = app.db"));

var app = builder.Build();

app.UseResponseCompression();
app.MapControllers();
app.MapProductEndpoints();

app.Run();
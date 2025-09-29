using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Module_01;
using Module_01.Repositories;
using Module_01.Endpoints;
using Module_01.Interfaces;

var builder = WebApplication.CreateBuilder(args);

/*

builder.Services.AddScoped<DapperProductRepository>();

builder.Services.AddScoped<IDbConnection>(_ => new SqliteConnection("Data Source=app.db"));

builder.Services.AddScoped<IProductRepository, DapperProductRepository>();

*/

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProductRepository, EFProductRepository>();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source = app.db"));
builder.Services.Configure<JsonOptions>(options => options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);

var app = builder.Build();

/*

using var scope = app.Services.CreateScope();

var db = scope.ServiceProvider.GetRequiredService<IDbConnection>();

DatabaseInitializer.Initialize(db);

SqlMapper.AddTypeHandler(new GuidHandler());

*/

app.MapProductEndpoints();

app.Run();

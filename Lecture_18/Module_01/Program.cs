using Microsoft.Extensions.Logging.Console;
using Module_01.Services;

var builder = WebApplication.CreateBuilder(args);

/*

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Error);
builder.Logging.AddFilter("Microsoft.Hosting.Lifetime", LogLevel.Trace);

builder.Logging.AddFilter<ConsoleLoggerProvider>((category, level) =>
{
    if (category is null && category!.StartsWith("Module_01.Services"))
        return level >= LogLevel.Trace;

    return level >= LogLevel.Error;
});

*/

builder.Services.AddControllers();
builder.Services.AddScoped<OrderService>();

var app = builder.Build();

app.MapControllers();

app.Run();

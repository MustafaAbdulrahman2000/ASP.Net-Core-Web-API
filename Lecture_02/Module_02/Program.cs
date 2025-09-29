var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/{key}", (string key, IConfiguration config) => 
{
    return config[key];
});

app.Run();

using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

var data = new Dictionary<string, string?>
{
    { "Faculty", "Faculty Of Computer and Information Science." }
};

builder.Configuration.AddJsonFile(path: "test.json", optional: false, reloadOnChange: true);
builder.Configuration.AddIniFile(path: "test.ini", optional: false);
builder.Configuration.AddXmlFile(path: "test.xml", optional: false, reloadOnChange: true);

builder.Configuration.AddInMemoryCollection(data);

// builder.Services.Configure<Info>(builder.Configuration.GetSection(Info.Information));
builder.Services.AddOptions<Info>().Bind(builder.Configuration.GetSection(Info.Information));

var app = builder.Build();

app.MapGet("/{key}", (string key, IConfiguration config) => 
{
    return config[key];
});

app.MapGet("/ini/{key}", (string key, IConfiguration config) =>
{
    return config[key];
});

app.MapGet("/in-memory/{key}", (string key, IConfiguration config) =>
{
    return config[key];
});


// =====================================================================


app.MapGet("/get-value-by-key", (IConfiguration config) => 
{
    return config["Name"];
});

app.MapGet("/get-value-by-path", (IConfiguration config) =>
{
    return config["ConnectionStrings:DefaultConnection"];
});

app.MapGet("/get-connection-string", (IConfiguration config) =>
{
    return config.GetConnectionString("DefaultConnection");
});

app.MapGet("/get-value", (IConfiguration config) =>
{
    return config.GetValue<string>("Name");
});

app.MapGet("/get", (IConfiguration config) =>
{
    return config.GetSection(Info.Information).Get<Info>();
});

app.MapGet("/bind", (IConfiguration config) =>
{
    var info = new Info();

    config.GetSection(Info.Information).Bind(info);

    return info;
});


// =====================================================================

app.Map("/ioptions", (IOptions<Info> options) =>
{
    return options.Value;
});

app.Map("/ioptions-snapshot", (IOptionsSnapshot<Info> options) => 
{
    return options.Value;
});

app.Map("/ioptions-monitor", (IOptionsMonitor<Info> options) =>
{
    return options.CurrentValue;
});

app.Run();


public class Info
{
    public const string Information = "Info";
    public string? Name { get; set; }
    public int Age { get; set; }
    public string? Faculty { get; set; }
    public bool Graduated { get; set; }
}
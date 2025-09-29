using Module_01.Filters;
using Module_01.Filters.EndpointFilters;
using Module_01.Filters.ExceptionFilters;
using Module_01.Filters.ResourceFilters;
using Module_01.Filters.ResultFilters;

var builder = WebApplication.CreateBuilder(args);

/*

builder.Services.AddControllers(opt => 
{
    opt.Filters.Add<SampleActionFilter>();
    opt.Filters.Add<SampleActionFilterAsync>();
    opt.Filters.Add<TrackActionTimeFilter>();
    opt.Filters.Add<TrackActionTimeFilterV2>();
    opt.Filters.Add<TrackActionTimeFilterV3>();
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<TenantValidationFilter>();
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<EnvelopeResultFilter>();
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});

app.MapControllers();

*/

var app = builder.Build();

/*

app.MapGet("/api/products", () => new[] { "Keyboard [$58.99]", "Mouse [$39.99]" })
   .AddEndpointFilter<EnvelopeResultEndpointFilter>()
   .AddEndpointFilter<TrackActionTimeEndpointFilter>();

*/

var globalGroup = app.MapGroup("")
                     .AddEndpointFilter<EnvelopeResultEndpointFilter>()
                     .AddEndpointFilter<TrackActionTimeEndpointFilter>();

globalGroup.MapGet("/api/products", () => new[] { "Keyboard [$58.99]", "Mouse [$39.99]" });
globalGroup.MapGet("/api/customers", () => new[] { "Ahmed [HR]", "Amr [Finance]" });

app.Run();
using Module_02.Data;
using Asp.Versioning;
using Module_01.Endpoints.V1;
using Module_01.Endpoints.V2;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ProductRepository>();

/*

builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = new QueryStringApiVersionReader("api-version");
});

builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = new HeaderApiVersionReader("api-version");
});

*/

builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = new MediaTypeApiVersionReader("v");
});

var app = builder.Build();

var apiVersionSet = app.NewApiVersionSet()
                       .HasApiVersion(new ApiVersion(1, 0))
                       .HasApiVersion(new ApiVersion(2, 0))
                       .ReportApiVersions()
                       .Build();

app.MapProductEndpointsV1(apiVersionSet);
app.MapProductEndpointsV2(apiVersionSet);

app.Run();
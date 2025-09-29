using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Asp.Versioning;
using Module_01.Data;
using Module_01.Services;
using Module_01;
using Module_01.Endpoints;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

app.UseAuthentication();
app.UseAuthorization();

var apiVersionSet = app.NewApiVersionSet()
                       .HasApiVersion(new ApiVersion(1, 0))
                       .HasApiVersion(new ApiVersion(2, 0))
                       .ReportApiVersions()
                       .Build();

var versionedGroup = app.MapGroup("/api/v{apiVersion:apiVersion}")
                        .WithApiVersionSet(apiVersionSet);


versionedGroup.MapProjectEndpoints();  // untouched

app.MapTokenEndpoints();

/*

    app.MapControllers();

*/

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Project API V1");
        options.SwaggerEndpoint("/openapi/v2.json", "Project API V2");

        options.EnableDeepLinking();
        options.DisplayRequestDuration();
        options.EnableFilter();

        app.MapScalarApiReference();
    });
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    await AppDbContextInitializer.InitializeAsync(context);
}

app.Run();
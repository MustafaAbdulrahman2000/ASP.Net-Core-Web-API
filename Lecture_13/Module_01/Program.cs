using Module_01.Endpoints;
using Module_01.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails(opt => 
{
    opt.CustomizeProblemDetails = (context) =>
    {
        context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
        context.ProblemDetails.Extensions.Add("requestId", context.HttpContext.TraceIdentifier);
    };
});

/*

builder.Services.AddProblemDetails();

*/

var app = builder.Build();

/*

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
}
else 
{
    app.UseExceptionHandler("/error-development");
}

app.UseExceptionHandler();
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

*/

app.UseExceptionHandler();
app.UseStatusCodePages();

app.MapControllers();
app.MapErrorEndpoints();

app.Run();

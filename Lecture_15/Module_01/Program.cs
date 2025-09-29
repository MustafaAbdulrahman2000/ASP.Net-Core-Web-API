using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();

builder.Services.AddAuthorization(opt => 
{
    opt.AddPolicy("supervisor-with-driver-license-A", policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, "supervisor");
        policy.RequireClaim("driver-license-class", "A");
    });
});

/*

builder.Services.AddAuthentication()
                .AddCookie();

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie();

builder.Services.AddAuthentication("Cookies")
                .AddCookie();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();

*/

var app = builder.Build();

/*

app.UseAuthentication();

app.MapGet("/login", async (HttpContext httpContext) => 
{
    List<Claim> claims =
    [
        new ("name", "Mustafa Ahmed"),
        new ("email", "ahmos2970@gmail.com"),
        new ("sub", Guid.NewGuid().ToString())
    ];

    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

    var principal = new ClaimsPrincipal(identity);

    await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

});

app.MapGet("/logout", async (HttpContext httpContext) =>
{
    await httpContext.SignOutAsync();
});

app.MapGet("/user", (HttpContext httpContext) =>
{
    var principal = httpContext.User;

    if (principal.Identity is { IsAuthenticated: true })
    {
        var claims = principal.Claims.Select(c => new { c.Type, c.Value });

        return Results.Ok(claims);
    }

    return Results.Unauthorized();
});

*/

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/login", async (HttpContext httpContext) => 
{
    List<Claim> claims =
    [
        new ("name", "Mustafa Ahmed"),
        new ("email", "ahmos2970@gmail.com"),
        new ("sub", Guid.NewGuid().ToString()),
        new ("driver-license-class", "A"),
        new (ClaimTypes.Role, "supervisor")
    ];

    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

    var principal = new ClaimsPrincipal(identity);

    await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

});

app.MapGet("/logout", async (HttpContext httpContext) =>
{
    await httpContext.SignOutAsync();
});

app.MapGet("/user", [Authorize] (HttpContext httpContext) =>
{
    var principal = httpContext.User;

    var claims = principal.Claims.Select(c => new { c.Type, c.Value });

    return Results.Ok(claims);
});

app.MapGet("/secure",  (HttpContext httpContext) =>
{
    return Results.Ok("Secure Page.");
}).RequireAuthorization();

app.MapGet("/supervisor-only",  (HttpContext httpContext) =>
{
    return Results.Ok("Supervisor or Admin Page.");
}).RequireAuthorization(a => a.RequireRole("admin", "supervisor"));

app.MapGet("/admin-only",  (HttpContext httpContext) =>
{
    return Results.Ok("Admin Page.");
}).RequireAuthorization(a => a.RequireRole("admin"));

app.MapGet("/account/login", () => "Login Page");

app.MapPost("/drive/bus", () =>
{
    return Results.Ok("Only class A driver can drive bus.");
}).RequireAuthorization("supervisor-with-driver-license-A");

app.Run();

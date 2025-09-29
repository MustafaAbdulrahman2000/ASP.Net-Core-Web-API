using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Module_02.Permissions;
using Module_02.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
       .AddControllers();

builder.Services
       .AddScoped<JwtTokenProvider>();

builder.Services
       .AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
       .AddJwtBearer(opt => 
{
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    opt.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!))
    };
});

builder.Services
       .AddAuthorization(opt => 
{
    opt.AddPolicy(Permission.Project.Create, policy => policy.RequireClaim("permission", Permission.Project.Create));
    opt.AddPolicy(Permission.Project.Read, policy => policy.RequireClaim("permission", Permission.Project.Read));
    opt.AddPolicy(Permission.Project.Update, policy => policy.RequireClaim("permission", Permission.Project.Update));
    opt.AddPolicy(Permission.Project.Delete, policy => policy.RequireClaim("permission", Permission.Project.Delete));
    opt.AddPolicy(Permission.Project.AssignMember, policy => policy.RequireClaim("permission", Permission.Project.AssignMember));
    opt.AddPolicy(Permission.Project.ManageBudget, policy => policy.RequireClaim("permission", Permission.Project.ManageBudget));

    opt.AddPolicy(Permission.Task.Create, policy => policy.RequireClaim("permission", Permission.Task.Create));
    opt.AddPolicy(Permission.Task.Read, policy => policy.RequireClaim("permission", Permission.Task.Read));
    opt.AddPolicy(Permission.Task.Update, policy => policy.RequireClaim("permission", Permission.Task.Update));
    opt.AddPolicy(Permission.Task.Delete, policy => policy.RequireClaim("permission", Permission.Task.Delete));
    opt.AddPolicy(Permission.Task.AssignUser, policy => policy.RequireClaim("permission", Permission.Task.AssignUser));
    opt.AddPolicy(Permission.Task.UpdateStatus, policy => policy.RequireClaim("permission", Permission.Task.UpdateStatus));
    opt.AddPolicy(Permission.Task.Comment, policy => policy.RequireClaim("permission", Permission.Task.Comment));    
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

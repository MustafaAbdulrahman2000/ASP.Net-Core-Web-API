using Microsoft.AspNetCore.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Module_05.Data;
using Module_05.Services;
using Module_05.Requests;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IBiddingService, BiddingService>();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=app.db"));
builder.Services.AddDataProtection()
                .PersistKeysToDbContext<AppDbContext>();

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
});

var app = builder.Build();

app.MapGet("/api/bids", async (IBiddingService service) =>
{
    return Results.Ok(await service.GetAllBidsAsync());
});

app.MapGet("/api/bids/{bidId:guid}", async (Guid bidId, IBiddingService service) =>
{
    var bid = await service.GetBidAsync(bidId);

    if (bid is null)
        return Results.NotFound($"Bid with Id '{bidId}' not found.");

    return Results.Ok(bid);
});

app.MapPost("/api/bids", async (CreateBidRequest request, IBiddingService service) =>
{
    var bid = await service.CreateBidAsync(request);

    return Results.Created($"/api/bids/{bid.Id}", bid);
});

app.Run();
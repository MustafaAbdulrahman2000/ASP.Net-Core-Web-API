using Module_01.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddHostedService<BlobStorageCleanupTraditionalTimerService>();

builder.Services.AddHostedService<BlobStorageCleanupPeriodicTimerService>();

var app = builder.Build();

app.Run();

namespace Module_01.BackgroundServices;

public class BlobStorageCleanupTraditionalTimerService(ILogger<BlobStorageCleanupTraditionalTimerService> logger): BackgroundService
{
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(10);
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Cleanup service is started at {time}", DateTimeOffset.UtcNow);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                logger.LogInformation("Scanning for orphaned blobs ...");

                await Task.Delay(1000, stoppingToken);

                var orphanedBlobs = Random.Shared.Next(1, 10);

                logger.LogInformation("Deleted {items} orphaned blobs at {time}", [orphanedBlobs, DateTimeOffset.UtcNow]);

            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Invalid operation");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }   
}

namespace Module_01.BackgroundServices;

public class BlobStorageCleanupPeriodicTimerService(ILogger<BlobStorageCleanupPeriodicTimerService> logger) : BackgroundService
{
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(5);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Cleanup service is started at {time}", DateTimeOffset.UtcNow);

        var periodicTimer = new PeriodicTimer(_interval);

        while (await periodicTimer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                logger.LogInformation("Scanning for orphaned blobs ...");

                await Task.Delay(1000, stoppingToken);

                var orphanedBlobs = Random.Shared.Next(1, 10);

                logger.LogInformation("Deleted {items} orphaned blobs at {time}", [orphanedBlobs, DateTimeOffset.UtcNow]);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Invalid operation");
            }
        }
    }
}

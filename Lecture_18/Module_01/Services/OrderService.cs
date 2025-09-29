namespace Module_01.Services;

public class OrderService(ILogger<OrderService> logger)
{
    public Task ProcessOrder(Guid orderId, Guid userId)
    {
        /*
        
        logger.LogInformation("Order with id '{OrderId}' is processed", orderId);
        

        logger.LogError($"Unstructured error log for user '{userId}' with order '{orderId}'");
        logger.LogError("Error log for user '{UserId}' with order '{OrderId}'", userId, orderId);

        */

        logger.Log(LogLevel.Trace, "Trace log for order with id '{OrderId}'", orderId);
        logger.Log(LogLevel.Debug, "Debug log for order with id '{OrderId}'", orderId);
        logger.Log(LogLevel.Information, "Information log for order with id '{OrderId}'", orderId);
        logger.Log(LogLevel.Warning, "Warning log for order with id '{OrderId}'", orderId);
        logger.Log(LogLevel.Error, "Error log for order with id '{OrderId}'", orderId);
        logger.Log(LogLevel.Critical, "Critical log for order with id '{OrderId}'", orderId);

        return Task.CompletedTask;
    }
}

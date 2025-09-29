using Microsoft.AspNetCore.Mvc.Filters;

namespace Module_01.Filters.ActionFilters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class TrackActionTimeFilterV2: Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        System.Console.WriteLine("Track action time filter started.");

        context.HttpContext.Items["ActionStartTime"] = DateTime.UtcNow;

        await next();

        var startTime = (DateTime)context.HttpContext.Items["ActionStartTime"]!;
        var elapsed = DateTime.UtcNow - startTime;

        context.HttpContext.Response.Headers.Append("X-Elapsed-Time", $"{elapsed.TotalMilliseconds}ms");

        System.Console.WriteLine($"Track action time filter took {elapsed.TotalMilliseconds}ms.");
    }
}
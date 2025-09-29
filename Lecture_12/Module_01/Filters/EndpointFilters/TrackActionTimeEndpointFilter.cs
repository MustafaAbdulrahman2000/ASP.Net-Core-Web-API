namespace Module_01.Filters.EndpointFilters;

public class TrackActionTimeEndpointFilter: IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var start = DateTime.UtcNow;

        var result = await next(context);

        var elapsed = DateTime.UtcNow - start;

        context.HttpContext.Response.Headers.Append("X-Elapsed-Time", $"{elapsed.TotalMilliseconds}ms");

        System.Console.WriteLine($"Track time filter took {elapsed}ms");

        return result;
    }    
}
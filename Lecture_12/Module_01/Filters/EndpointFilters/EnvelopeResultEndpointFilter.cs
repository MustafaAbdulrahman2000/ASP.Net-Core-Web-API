namespace Module_01.Filters.EndpointFilters;

public class EnvelopeResultEndpointFilter: IEndpointFilter
{   
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var result = await next(context);

        return Results.Json(new
        {
            success = true,
            data = result
        });
    }
}
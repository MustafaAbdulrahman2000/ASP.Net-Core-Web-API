using Microsoft.AspNetCore.Mvc.Filters;

namespace Module_01.Filters.ActionFilters;

public class SampleActionFilterAsync : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        System.Console.WriteLine("Sample action filter async before.");

        await next();

        System.Console.WriteLine("Sample action filter async after.");
    }
}

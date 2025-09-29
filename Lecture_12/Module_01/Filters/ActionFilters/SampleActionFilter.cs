using Microsoft.AspNetCore.Mvc.Filters;

namespace Module_01.Filters.ActionFilters;

public class SampleActionFilter: IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
        System.Console.WriteLine("Sample action filter sync before.");
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        System.Console.WriteLine("Sample action filter sync after.");
    }
}
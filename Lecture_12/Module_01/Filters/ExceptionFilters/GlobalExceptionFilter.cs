using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Module_01.Filters.ExceptionFilters;

public class GlobalExceptionFilter: IAsyncExceptionFilter
{
    public Task OnExceptionAsync(ExceptionContext context)
    {
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Internal Server Error",
            Detail = context.Exception.Message
        };

        context.Result = new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };

        context.ExceptionHandled = true;

        return Task.CompletedTask;
    }
}
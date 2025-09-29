using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Module_01.Filters.ResultFilters;

public class EnvelopeResultFilter: IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is ObjectResult objectResult && objectResult.Value is not null)
        {
            var wrapped = new
            {
                success = true,
                data = objectResult.Value
            };

            context.Result = new JsonResult(wrapped)
            {
                StatusCode = objectResult.StatusCode
            };
        }
        
        await next();
    }
}
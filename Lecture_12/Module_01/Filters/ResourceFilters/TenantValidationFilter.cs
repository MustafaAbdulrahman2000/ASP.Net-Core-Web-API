using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Module_01.Filters.ResourceFilters;

public class TenantValidationFilter(IConfiguration configuration): IAsyncResourceFilter
{
    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {
        var tenantId = context.HttpContext.Request.Headers["tenantId"].ToString();
        var apiKey = context.HttpContext.Request.Headers["x-api-key"].ToString();

        var expectedKey = configuration[$"Tenants:{tenantId}:ApiKey"];
        
        if (string.IsNullOrEmpty(expectedKey) || expectedKey != apiKey)
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        
        await next();
    }
}
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Module_01;

public static class RouteHandlerBuilderExtensions
{
    public static RouteHandlerBuilder Validate<T>(this RouteHandlerBuilder source)
    {
        source.AddEndpointFilter(async (context, next) =>
        {
            var argument = context.Arguments.OfType<T>().FirstOrDefault();

            if (argument is null)
            {
                return Results.Problem(new ProblemDetails
                {
                    Title = "Bad Request",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = $"{nameof(T)} is null"
                });
            }

            List<ValidationResult> validationResults = [];

            var isValid = Validator.TryValidateObject(argument, new ValidationContext(argument), validationResults, true);

            if (!isValid)
            {
                var errorGroups = validationResults.SelectMany(v => (v.MemberNames.Any() ? v.MemberNames : new[] { "" })
                                                    .Select(name => new { name, v.ErrorMessage }))
                                                    .GroupBy(x => x.name)
                                                    .ToDictionary(
                                                        g => g.Key,
                                                        g => g.Select(x => x.ErrorMessage!).ToArray()
                                                    );

                return Results.ValidationProblem(errorGroups);
            }

            return await next(context);
        });

        return source;
    }

}

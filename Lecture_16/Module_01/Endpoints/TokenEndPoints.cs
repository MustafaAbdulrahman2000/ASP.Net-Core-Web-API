using Module_01.Identity;
using Module_01.Requests;
using Module_01.Responses;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Module_01.Endpoints;

public static class TokenEndpoints
{
    public static RouteGroupBuilder MapTokenEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("token");

        group.MapPost("/generate", GenerateToken);

        group.MapPost("/refresh-token", RefreshToken);
        
        return group;
    }

    private static Results<Ok<TokenResponse>, ProblemHttpResult> GenerateToken(
        GenerateTokenRequest request,
        JwtTokenProvider tokenProvider)
    {
        var token = tokenProvider.GenerateJwtToken(request);
        return TypedResults.Ok(token);
    }
    private static Results<Ok<TokenResponse>, ProblemHttpResult> RefreshToken(
        RefreshTokenRequest request,
        JwtTokenProvider tokenProvider)
    {
        var refreshTokenRecord = new
        {
            UserId = "79410514-0136-4442-be9b-01f097c57f7a",
            RefreshToken = "7a6f23b4e1d04c9a8f5b6d7c8a9e01f1",
            Expires = DateTime.UtcNow.AddHours(12)
        };

        if (request.RefreshToken != refreshTokenRecord.RefreshToken ||
            refreshTokenRecord.Expires < DateTime.UtcNow)
        {
            return TypedResults.Problem(
                title: "Bad Request",
                statusCode: StatusCodes.Status400BadRequest,
                detail: "Refresh token is invalid and/or has expired"
            );
        }

        var user = new
        {
            Id = "79410514-0136-4442-be9b-01f097c57f7a",
            FirstName = "Primary",
            LastName = "Manager",
            Email = "pm@localhost",
            Permissions = new List<string>
            {
                "project:create", "project:read", "project:update", "project:delete",
                "project:assign_member", "project:manage_budget",
                "task:create", "task:read", "task:update", "task:delete",
                "task:assign_user", "task:update_status"
            },
            Roles = new List<string> { "ProjectManager" }
        };

        var tokenRequest = new GenerateTokenRequest
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Roles = user.Roles,
            Permissions = user.Permissions
        };

        var token = tokenProvider.GenerateJwtToken(tokenRequest);
        return TypedResults.Ok(token);
    }
}
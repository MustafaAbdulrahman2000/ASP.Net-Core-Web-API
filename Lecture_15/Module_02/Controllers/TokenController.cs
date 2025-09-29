using Microsoft.AspNetCore.Mvc;
using Module_02.Requests;
using Module_02.Services;

namespace Module_02.Controllers;

[ApiController]
[Route("api/token")]
public class TokenController(JwtTokenProvider jwtTokenProvider): ControllerBase
{

    [HttpPost("generate")]
    public IActionResult GenerateToken(GenerateTokenRequest request)
    {
        return Ok(jwtTokenProvider.GenerateJwtToken(request));
    }

    [HttpPost("refresh-token")]
    public IActionResult GenerateToken(RefreshTokenRequest request)
    {
        var refreshTokenRecord = new
        {
            UserId = "79410514-0136-4442-be9b-01f097c57f7a",
            RefreshToken = "7a6f23b4e1d04c9a8f5b6d7c8a9e01f1",
            Expires = DateTime.UtcNow.AddMinutes(1)
        };

        if (request.RefreshToken != "7a6f23b4e1d04c9a8f5b6d7c8a9e01f1" || refreshTokenRecord is null || refreshTokenRecord.Expires < DateTime.UtcNow)
        {
            return Problem(
                title: "Bad Request",
                statusCode: StatusCodes.Status400BadRequest,
                detail: "Refresh token is invalid and/or has expired."
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
                "project:create",
                "project:read",
                "project:update",
                "project:delete",
                "project:assign_member",
                "project:manage_budget",
                "task:create",
                "task:read",
                "task:update",
                "task:delete",
                "task:assign_user",
                "task:update_status"
            },
            Roles = new List<string>
            { 
                "ProjectManager"
            }
        };

        var generateTokenRequest = new GenerateTokenRequest
        {
            Id = refreshTokenRecord.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Roles = user.Roles,
            Permissions = user.Permissions
        };

        return Ok(jwtTokenProvider.GenerateJwtToken(generateTokenRequest));
    }
}

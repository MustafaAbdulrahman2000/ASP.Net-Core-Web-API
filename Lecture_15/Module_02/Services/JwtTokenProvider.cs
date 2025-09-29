using Module_02.Responses;
using Module_02.Requests;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Module_02.Services;

public class JwtTokenProvider(IConfiguration configuration)
{
    public TokenResponse GenerateJwtToken(GenerateTokenRequest request)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");

        var issuer = jwtSettings["Issuer"]!;
        var audienec = jwtSettings["Audience"]!;
        var key = jwtSettings["SecretKey"]!;
        var expiry = DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["TokenExpirationInMinutes"]!));

        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, request.Id!),
            new (JwtRegisteredClaimNames.Email, request.Email!),
            new (JwtRegisteredClaimNames.FamilyName, request.LastName!),
            new (JwtRegisteredClaimNames.GivenName, request.FirstName!)
        };

        foreach(var role in request.Roles)
            claims.Add(new(ClaimTypes.Role, role));

        foreach(var permission in request.Permissions)
            claims.Add(new("Permission", permission));

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiry,
            Issuer = issuer,
            Audience = audienec,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var securityToken = tokenHandler.CreateToken(descriptor);

        return new TokenResponse
        {
            AccessToken = tokenHandler.WriteToken(securityToken), 
            RefreshToken = "7a6f23b4e1d04c9a8f5b6d7c8a9e01f1",
            Expires = expiry
        };
    }
}

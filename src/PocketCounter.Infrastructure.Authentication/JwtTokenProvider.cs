using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PocketCounter.Application.Authorization;
using PocketCounter.Application.Authorization.DataModels;

namespace PocketCounter.Infrastructure.Authentication;

public class JwtTokenProvider : ITokenProvider
{
    private readonly JwtOptions _jwtOptions;

    public JwtTokenProvider(IOptions<JwtOptions> options)
    {
        _jwtOptions = options.Value;
    }

    public string GenerateAccessToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));

        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        Claim[] claims =
        [
            new Claim(CustomClaims.Sub, user.Id.ToString()),
            new Claim(CustomClaims.Email, user.Email ?? "")/*,
            new Claim(ClaimTypes.Role, )*/
        ];
        var jwtToken = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_jwtOptions.ExpiredMinutesTime)),
            signingCredentials: signingCredentials,
            claims: claims);

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}
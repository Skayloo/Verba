using Verba.Abstractions.Application.Authorization;
using Verba.Abstractions.Application.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Verba.Core.Application.JwtConfiguration;

public class JwtTokenService : IJwtTokenService
{
    private readonly string _authorizationScheme = "bearer ";
    private readonly IOptions<JwtAuthSettings> _options;

    public JwtTokenService(IOptions<JwtAuthSettings> options)
    {
        _options = options;
    }

    public string GenerateJwtAuthTokenAsync(string email, string userId, IEnumerable<string> roles)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.JwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddMinutes(_options.Value.JwtExpirationMinutes);

        var token = new JwtSecurityToken(
            _options.Value.JwtIssuer,
            _options.Value.JwtIssuer,
            CreateClaims(email, userId, roles),
            expires: expires,
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateJwtAuthTokenWithPermissionsAsync(string email, string userId, IEnumerable<string> permissions)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.JwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddMinutes(_options.Value.JwtExpirationMinutes);

        var token = new JwtSecurityToken(
            _options.Value.JwtIssuer,
            _options.Value.JwtIssuer,
            CreateClaimsWithPermissions(email, userId, permissions),
            expires: expires,
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public JwtSecurityToken ParseJwtAuthTokenAsync(string jwt)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = _options.Value.JwtIssuer,
            ValidAudience = _options.Value.JwtIssuer,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.JwtKey)),
            ClockSkew = TimeSpan.Zero
        };

        new JwtSecurityTokenHandler().ValidateToken(jwt, tokenValidationParameters, out var securityToken);
        return (JwtSecurityToken)securityToken;
    }

    public string GetUserIdFromJwtAuthToken(string jwt)
    {
        jwt = Regex.Replace(jwt, Regex.Escape(_authorizationScheme), string.Empty, RegexOptions.IgnoreCase);
        var jwtSecurityToken = ParseJwtAuthTokenAsync(jwt);
        return jwtSecurityToken.Claims.SingleOrDefault(x => x.Type.Contains("nameidentifier"))?.Value;
    }

    public string GetUserEmailFromJwtAuthToken(string jwt)
    {
        jwt = Regex.Replace(jwt, Regex.Escape(_authorizationScheme), string.Empty, RegexOptions.IgnoreCase);
        var jwtSecurityToken = ParseJwtAuthTokenAsync(jwt);
        return jwtSecurityToken.Claims.SingleOrDefault(x => x.Type.Contains("sub"))?.Value;
    }

    public string GetUserRoleFromJwtAuthToken(string jwt)
    {
        jwt = Regex.Replace(jwt, Regex.Escape(_authorizationScheme), string.Empty, RegexOptions.IgnoreCase);
        var jwtSecurityToken = ParseJwtAuthTokenAsync(jwt);
        return jwtSecurityToken.Claims.SingleOrDefault(x => x.Type.Contains("role"))?.Value;
    }

    public IEnumerable<string> GetUserPermissionsFromJwtAuthToken(string jwt)
    {
        jwt = Regex.Replace(jwt, Regex.Escape(_authorizationScheme), string.Empty, RegexOptions.IgnoreCase);
        var jwtSecurityToken = ParseJwtAuthTokenAsync(jwt);
        return jwtSecurityToken.Claims.Where(x => x.Type.Contains("Permissions")).Select(x => x.Value);
    }

    private static IEnumerable<Claim> CreateClaimsWithPermissions(string email, string userId, IEnumerable<string> permissions)
    {
        var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, ((DateTimeOffset) DateTime.Now).ToUnixTimeSeconds().ToString())
            };

        claims.AddRange(permissions.Select(permission => new Claim("Permissions", permission)));
        return claims.ToArray();
    }

    private static IEnumerable<Claim> CreateClaims(string email, string userId, IEnumerable<string> roles)
    {
        var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, ((DateTimeOffset) DateTime.Now).ToUnixTimeSeconds().ToString())
            };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        //claims.AddRange(permissions.Select(permission => new Claim("Permissions", permission)));
        return claims.ToArray();
    }
}

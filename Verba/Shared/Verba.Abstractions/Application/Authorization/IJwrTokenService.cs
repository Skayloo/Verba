using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verba.Abstractions.Application.Authorization;

public interface IJwtTokenService
{
    string GenerateJwtAuthTokenAsync(string email, string userId, IEnumerable<string> roles);

    string GenerateJwtAuthTokenWithPermissionsAsync(string email, string userId, IEnumerable<string> permissions);

    JwtSecurityToken ParseJwtAuthTokenAsync(string jwt);

    string GetUserIdFromJwtAuthToken(string jwt);

    string GetUserEmailFromJwtAuthToken(string jwt);

    string GetUserRoleFromJwtAuthToken(string jwt);

    IEnumerable<string> GetUserPermissionsFromJwtAuthToken(string jwt);
}

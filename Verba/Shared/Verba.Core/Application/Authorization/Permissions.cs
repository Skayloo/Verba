using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Verba.Core.Application.Authorization;

public static class Permissions
{
    public const string User = "User";
    public const string Admin = "Admin";
    public const string Uploader = "Uploader";
    public const string Any = "Any";

    public static IEnumerable<string> GetFrom(IEnumerable<Claim> claims)
    {
        return claims.Where(x => x.Type.Contains("Permissions")).Select(x => x.Value);
    }
}

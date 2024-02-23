using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;

namespace Verba.Core.Application.Authentication;

public static class MultipleAuthentificatorsExtension
{
    public static void AllowBothJwtAndApiKeyAuthentification(this IApplicationBuilder app)
    {
        app.Use(async (context, next) =>
        {
            var principal = new ClaimsPrincipal();

            var result1 = await context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
            if (result1?.Principal != null)
            {
                principal.AddIdentities(result1.Principal.Identities);
            }

            context.User = principal;

            await next();
        });
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Verba.Abstractions.Application.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Verba.Core.Application.JwtConfiguration;

public static class JwtAuthServiceCollectionExtensions
{
    /// <summary>
    /// Added JWT-authorization in project
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void AddJwtAuthentification(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IJwtTokenService, JwtTokenService>();

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration["JwtAuth:JwtIssuer"],
                    ValidAudience = configuration["JwtAuth:JwtIssuer"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtAuth:JwtKey"])),
                    ClockSkew = TimeSpan.Zero
                };
            })
            .AddCookie();
    }

    public static void AddJwtAuthTokenInCookie(this IHttpContextAccessor contextAccessor, string token)
    {
        var option = new CookieOptions { HttpOnly = true, Expires = DateTime.Now.AddYears(1) };
        contextAccessor.HttpContext.Response.Cookies.Delete("access_token");
        contextAccessor.HttpContext.Response.Cookies.Append("access_token", token, option);
    }
}

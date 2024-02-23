using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verba.Core.Application.JwtConfiguration;

public class JwtAuthInHeaderMiddleware
{
    private readonly RequestDelegate _next;

    public JwtAuthInHeaderMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var authenticationCookieName = "access_token";
        var token = context.Request.Cookies[authenticationCookieName];
        if (token != null)
        {
            context.Request.Headers.Append("Authorization", "Bearer " + token);
        }

        await _next.Invoke(context);
    }
}

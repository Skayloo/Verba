using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Verba.Core.Application.Authorization;

public static class PermissionAuthorizationAppExtension
{
    public static void AddPermissionsAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPermissionsPolicies();
            options.AddPolicy(Permissions.Any, policy =>
                              policy.RequireAuthenticatedUser()
                              .RequireClaim("Permissions"));
        });
    }

    private static void AddPermissionsPolicies(this AuthorizationOptions options)
    {
        Type type = typeof(Permissions);
        List<string> permissions = type.GetAllPublicConstantValues<string>();
        foreach (var permission in permissions)
        {
            if (permission == Permissions.Any) // TODO refactor
                continue;
            var policy = new AuthorizationPolicyBuilder()
                .BuildFor(permission);
            options.AddPolicy(permission, policy);
        }
    }

    private static List<T> GetAllPublicConstantValues<T>(this Type type)
    {
        return type
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(T))
            .Select(x => (T)x.GetRawConstantValue())
            .ToList();
    }

    private static AuthorizationPolicy BuildFor(this AuthorizationPolicyBuilder builder, string permission)
    {
        return builder.RequireAuthenticatedUser()
            .RequireClaim("Permissions", permission)
            .Build();
    }
}

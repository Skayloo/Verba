using System;
using Microsoft.Extensions.DependencyInjection;

namespace Verba.Core.Application.Autofac;

public static class AutofacServiceCollectionExtensions
{
    public static IServiceProvider AddAutofac(this IMvcBuilder mvcBuilder)
    {            
        var builder = new AutofacBuilder(mvcBuilder);
        return builder.GetServiceProvider();
    }
}
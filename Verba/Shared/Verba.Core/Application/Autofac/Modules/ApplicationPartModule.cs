using System;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Verba.Abstractions.Application.Modules;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Module = Autofac.Module;

namespace Verba.Core.Application.Autofac.Modules;

internal class ApplicationPartModule : Module
{
    private readonly IModuleProvider _moduleProvider;
    private readonly IMvcBuilder _mvcBuilder;
    private readonly ILogger _logger; 

    public ApplicationPartModule(IMvcBuilder mvcBuilder, IModuleProvider moduleProvider, ILoggerFactory loggerFactory)
    {
        _mvcBuilder = mvcBuilder;
        _moduleProvider = moduleProvider;
        _logger = loggerFactory.CreateLogger<ApplicationPartModule>();
    }

    protected override void Load(ContainerBuilder builder)
    {
        foreach (var assembly in _moduleProvider.ApplicationPartAssemblies)
        {
            _logger.LogInformation("ModuleProvider - {Module} {Message}", assembly.GetName().Name, "initialize starting...");

            _mvcBuilder.PartManager.ApplicationParts.Add(new AssemblyPart(assembly));

            var atrribute = assembly.GetCustomAttribute<ModuleAttribute>();
            var moduleStartup = (IModule)Activator.CreateInstance(atrribute.ModuleType);
            builder.RegisterModule(moduleStartup);

            _logger.LogInformation("ModuleProvider - {Module} {Message}", assembly.GetName().Name, "loading complited");
        }
    }
}
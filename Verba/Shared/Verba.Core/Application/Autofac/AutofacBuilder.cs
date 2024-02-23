using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Verba.Abstractions.Application.Modules;
using Verba.Core.Application.Autofac.Modules;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Verba.Core.Application.Autofac
{
    internal class AutofacBuilder
    {
        private readonly IMvcBuilder _mvcBuilder;
        
        public AutofacBuilder(IMvcBuilder mvcBuilder)
        {            
            _mvcBuilder = mvcBuilder;            
        }

        public IServiceProvider GetServiceProvider()
        {
            using (var provider = _mvcBuilder.Services.BuildServiceProvider())            
            {                
                var moduleProvider = provider.GetService<IModuleProvider>();
                var loggerFactory = provider.GetService<ILoggerFactory>();
                var containerBuilder = new ContainerBuilder();               
                containerBuilder.Populate(_mvcBuilder.Services);
                containerBuilder.RegisterModule<SharedModule>();
                containerBuilder.RegisterModule<MediatorModule>();
                containerBuilder.RegisterModule(new ApplicationPartModule(_mvcBuilder, moduleProvider, loggerFactory));               
                return new AutofacServiceProvider(containerBuilder.Build());
            }
        }
    }
}
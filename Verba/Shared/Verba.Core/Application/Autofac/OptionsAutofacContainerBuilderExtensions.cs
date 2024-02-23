using System;
using Autofac;
using Microsoft.Extensions.Options;

namespace FinFactory.Core.Application.Autofac
{
    public static class OptionsAutofacContainerBuilderExtensions
    {
        public static ContainerBuilder Configure<TOptions>(this ContainerBuilder builder, Action<TOptions> configureOptions) where TOptions : class
            => builder.Configure(Options.DefaultName, configureOptions);

        public static ContainerBuilder Configure<TOptions>(this ContainerBuilder builder, string name, Action<TOptions> configureOptions)
            where TOptions : class
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            builder.RegisterInstance(new ConfigureNamedOptions<TOptions>(name, configureOptions))
                .As<IConfigureOptions<TOptions>>().SingleInstance();            
            return builder;
        }
    }
}
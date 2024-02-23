using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Verba.Abstractions.Application.MediatR.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Module = Autofac.Module;


namespace Verba.Core.Application.Autofac.Modules;

public class MediatorModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
            .AsImplementedInterfaces();


        var services = new ServiceCollection();

        builder.Populate(services);
        //builder.Register<ServiceProvider>(ctx =>
        //{
        //    var componentContext = ctx.Resolve<IComponentContext>();
        //    return t => componentContext.TryResolve(t, out var o) ? o : null;

        //});

        builder.RegisterGeneric(typeof(ValidatorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
    }
}

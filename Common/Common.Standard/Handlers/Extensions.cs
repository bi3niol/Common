using Autofac;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Common.Standard.Handlers
{
    public static class Extensions
    {
        public static ContainerBuilder AddHandlers(this ContainerBuilder builder, params Assembly[] assemblies)
        {
            if (assemblies == null || assemblies.Length == 0)
                assemblies = new Assembly[] { Assembly.GetCallingAssembly() };

            builder.AddQueryHandlers(assemblies);
            builder.AddCommandHandlers(assemblies);
            builder.AddEventHandlers(assemblies);

            return builder;
        }
        public static ContainerBuilder AddQueryHandlers(this ContainerBuilder builder, params Assembly[] assemblies)
        {
            if (assemblies == null || assemblies.Length == 0)
                assemblies = new Assembly[] { Assembly.GetCallingAssembly() };

            builder.RegisterAssemblyTypes(assemblies)
             .AsClosedTypesOf(typeof(IQueryHandler<,>))
             .InstancePerDependency();

            return builder;
        }
        public static ContainerBuilder AddCommandHandlers(this ContainerBuilder builder, params Assembly[] assemblies)
        {
            if (assemblies == null || assemblies.Length == 0)
                assemblies = new Assembly[] { Assembly.GetCallingAssembly() };

            builder.RegisterAssemblyTypes(assemblies)
             .AsClosedTypesOf(typeof(ICommandHandler<>))
             .InstancePerDependency();

            return builder;
        }
        public static ContainerBuilder AddEventHandlers(this ContainerBuilder builder, params Assembly[] assemblies)
        {
            if (assemblies == null || assemblies.Length == 0)
                assemblies = new Assembly[] { Assembly.GetCallingAssembly() };

            builder.RegisterAssemblyTypes(assemblies)
             .AsClosedTypesOf(typeof(IEventHandler<>))
             .InstancePerDependency();

            return builder;
        }
    }
}

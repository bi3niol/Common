using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Standard.Dispatchers
{
    public static class Extensions
    {
        public static void AddDispatchers(this ContainerBuilder builder)
        {
            builder.RegisterType<CommandDispatcher>().As<ICommandDispatcher>();
            builder.RegisterType<Dispatcher>().As<IDispatcher>();
            builder.RegisterType<QueryDispatcher>().As<IQueryDispatcher>();
        }
    }
}

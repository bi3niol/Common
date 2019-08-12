using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Common.Standard.Handlers;
using Common.Standard.Messages;
using Common.Standard.Types;

namespace Common.Standard.Dispatchers
{
    internal class CommandDispatcher : ICommandDispatcher
    {
        public CommandDispatcher(IComponentContext context)
        {
            this.context = context;
        }

        private IComponentContext context { get; }
        public async Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            await context.Resolve<ICommandHandler<TCommand>>().HandleAsync(command, CorrelationContext.Empty);
        }
    }
}

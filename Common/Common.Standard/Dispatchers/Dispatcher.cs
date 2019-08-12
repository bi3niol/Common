using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Common.Standard.Messages;
using Common.Standard.Types;

namespace Common.Standard.Dispatchers
{
    internal class Dispatcher : IDispatcher
    {
        public Dispatcher(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        private IQueryDispatcher _queryDispatcher { get; }
        private ICommandDispatcher _commandDispatcher { get; }

        public Task<TResult> QueryAsync<TResult>(IQuery<TResult> query) => _queryDispatcher.QueryAsync(query);

        public Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand => _commandDispatcher.SendAsync(command);
    }
}

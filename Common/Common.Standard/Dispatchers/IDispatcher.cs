using Common.Standard.Messages;
using Common.Standard.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Standard.Dispatchers
{
    public interface IDispatcher
    {
        Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand;
        Task<TResult> QueryAsync<TResult>(IQuery<TResult> query);
    }
}

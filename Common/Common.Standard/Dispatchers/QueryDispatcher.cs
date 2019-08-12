using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Common.Standard.Handlers;
using Common.Standard.Types;

namespace Common.Standard.Dispatchers
{
    internal class QueryDispatcher : IQueryDispatcher
    {
        public QueryDispatcher(IComponentContext context)
        {
            this.context = context;
        }

        private IComponentContext context { get; } 
        public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query)
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            dynamic handler = context.Resolve(handlerType);
            return await handler.HandleAsync((dynamic)query);
        }
    }
}

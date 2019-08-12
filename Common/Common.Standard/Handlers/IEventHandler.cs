using Common.Standard.Messages;
using Common.Standard.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Standard.Handlers
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        Task HandleAsync(TEvent @event, ICorrelationContext correlationContext);
    }
}

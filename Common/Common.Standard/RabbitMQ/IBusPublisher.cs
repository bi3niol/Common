using Common.Standard.Messages;
using Common.Standard.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Standard.RabbitMQ
{
    public interface IBusPublisher
    {
        Task PublishAsync<TEvent>(TEvent @event, ICorrelationContext context) where TEvent : IEvent;
        Task SendAsync<TCommand>(TCommand command, ICorrelationContext context) where TCommand : ICommand;
    }
}

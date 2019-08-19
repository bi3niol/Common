using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Common.Standard.Messages;
using Common.Standard.Types;
using RawRabbit;
using RawRabbit.Enrichers.MessageContext;

namespace Common.Standard.RabbitMQ
{
    public class BusPublisher : IBusPublisher
    {
        private IBusClient _busClient;
        private string _defaultNamespace;

        public BusPublisher(IBusClient busClient, RabbitMqOptions options)
        {
            _busClient = busClient;
            _defaultNamespace = options.Namespace;
        }

        public Task PublishAsync<TEvent>(TEvent @event, ICorrelationContext context) where TEvent : IEvent
           =>
            _busClient.PublishAsync(@event, ctx => ctx.UseMessageContext(context).UsePublishConfiguration(
                p => p.WithRoutingKey(typeof(TEvent).GetRoutingKey(_defaultNamespace))));

        public Task SendAsync<TCommand>(TCommand command, ICorrelationContext context) where TCommand : ICommand
           =>
            _busClient.PublishAsync(command, ctx => ctx.UseMessageContext(context).UsePublishConfiguration(
                p => p.WithRoutingKey(typeof(TCommand).GetRoutingKey(_defaultNamespace))));
    }
}

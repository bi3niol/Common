using Common.Standard.Messages;
using Common.Standard.Types;
using System;

namespace Common.Standard.RabbitMQ
{
    public interface IBusSubscriber
    {
        IBusSubscriber SubscribeCommand<TCommand>(string @namespace = null, string queueName = null,
            Func<TCommand, CommonException, IRejectedEvent> onError = null) 
            where TCommand : ICommand;

        IBusSubscriber SubscribeEvent<TEvent>(string @namespace = null, string queueName = null,
            Func<TEvent, CommonException, IRejectedEvent> onError = null)
            where TEvent : IEvent;
    }
}

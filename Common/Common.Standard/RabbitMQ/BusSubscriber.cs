using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Common.Standard.Handlers;
using Common.Standard.Messages;
using Common.Standard.Types;
using RawRabbit;
using RawRabbit.Common;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Tag;
using RawRabbit.Enrichers.MessageContext;
using Microsoft.AspNetCore.Builder;

namespace Common.Standard.RabbitMQ
{
    public class BusSubscriber : IBusSubscriber
    {
        private IBusClient _busClient;
        private readonly IServiceProvider _serviceProvider;
        private ILogger _logger;
        private ITracer _tracer;

        private string _defaultNamespace;
        private int _retries;
        private double _retryInterval;

        public BusSubscriber(IApplicationBuilder app)
        {
            _logger = app.ApplicationServices.GetService<ILogger<BusSubscriber>>();
            _serviceProvider = app.ApplicationServices.GetService<IServiceProvider>();
            _busClient = _serviceProvider.GetService<IBusClient>();
            _tracer = _serviceProvider.GetService<ITracer>();
            var options = _serviceProvider.GetService<RabbitMqOptions>();
            _defaultNamespace = options.Namespace;
            _retries = options.Retries >= 0 ? options.Retries : 3;
            _retryInterval = options.RetryInterval > 0 ? options.RetryInterval : 2;
        }

        public IBusSubscriber SubscribeCommand<TCommand>(string @namespace = null, string queueName = null, Func<TCommand, CommonException, IRejectedEvent> onError = null) where TCommand : ICommand
        {
            @namespace = @namespace ?? _defaultNamespace;
            _busClient.SubscribeAsync<TCommand, CorrelationContext>(async (command, correlationcontext) =>
             {
                 var handler = _serviceProvider.GetService<ICommandHandler<TCommand>>();
                 return await TryHandleAsync(command, correlationcontext, () =>
                     handler.HandleAsync(command, correlationcontext) , onError);
             }, ctx => ctx.UseSubscribeConfiguration(cfg => cfg.FromDeclaredQueue(q => q.WithName(typeof(TCommand).GetQueueName(@namespace, queueName)))));
            return this;
        }

        public IBusSubscriber SubscribeEvent<TEvent>(string @namespace = null, string queueName = null, Func<TEvent, CommonException, IRejectedEvent> onError = null) where TEvent : IEvent
        {
            @namespace = @namespace ?? _defaultNamespace;
            _busClient.SubscribeAsync<TEvent, CorrelationContext>(async (@event, correlationcontext) =>
            {
                var handler = _serviceProvider.GetService<IEventHandler<TEvent>>();
                return await TryHandleAsync(@event, correlationcontext, () =>
                    handler.HandleAsync(@event, correlationcontext), onError);
            }, ctx => ctx.UseSubscribeConfiguration(cfg => cfg.FromDeclaredQueue(q => q.WithName(typeof(TEvent).GetQueueName(@namespace, queueName)))));
            return this;
        }

        private async Task<Acknowledgement> TryHandleAsync<TMessage>(TMessage message, CorrelationContext correlationContext,
           Func<Task> handle, Func<TMessage, CommonException, IRejectedEvent> onError = null)
        {
            var currentRetry = 0;
            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(_retries, i => TimeSpan.FromSeconds(_retryInterval));

            var messageName = message.GetType().Name;

            return await retryPolicy.ExecuteAsync<Acknowledgement>(async () =>
            {
                var scope = _tracer
                    .BuildSpan("executing-handler")
                    .AsChildOf(_tracer.ActiveSpan)
                    .StartActive(true);

                using (scope)
                {
                    var span = scope.Span;

                    try
                    {
                        var retryMessage = currentRetry == 0
                            ? string.Empty
                            : $"Retry: {currentRetry}'.";

                        var preLogMessage = $"Handling a message: '{messageName}' " +
                                      $"with correlation id: '{correlationContext.Id}'. {retryMessage}";

                        _logger.LogInformation(preLogMessage);
                        span.Log(preLogMessage);

                        await handle();

                        var postLogMessage = $"Handled a message: '{messageName}' " +
                                             $"with correlation id: '{correlationContext.Id}'. {retryMessage}";
                        _logger.LogInformation(postLogMessage);
                        span.Log(postLogMessage);

                        return new Ack();
                    }
                    catch (Exception exception)
                    {
                        currentRetry++;
                        _logger.LogError(exception, exception.Message);
                        span.Log(exception.Message);
                        span.SetTag(Tags.Error, true);

                        if (exception is CommonException commonException && onError != null)
                        {
                            var rejectedEvent = onError(message, commonException);
                            await _busClient.PublishAsync(rejectedEvent, ctx => ctx.UseMessageContext(correlationContext));
                            _logger.LogInformation($"Published a rejected event: '{rejectedEvent.GetType().Name}' " +
                                                   $"for the message: '{messageName}' with correlation id: '{correlationContext.Id}'.");

                            span.SetTag("error-type", "domain");
                            return new Ack();
                        }

                        span.SetTag("error-type", "infrastructure");
                        throw new Exception($"Unable to handle a message: '{messageName}' " +
                                            $"with correlation id: '{correlationContext.Id}', " +
                                            $"retry {currentRetry - 1}/{_retries}...");
                    }
                }
            });
        }
    }
}

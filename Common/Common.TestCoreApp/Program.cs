using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common.Standard.Handlers;
using Common.Standard.Messages;
using Common.Standard.RabbitMQ;
using Common.Standard.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Common.TestCoreApp
{
    public class TestEvent : IEvent
    {
        public int TestValue { get; set; }
    }

    class TestEventHandler : IEventHandler<TestEvent>
    {
        public async Task HandleAsync(TestEvent @event, ICorrelationContext correlationContext)
        {
            Console.WriteLine($"Handler : {this}\n TestVal : {@event.TestValue}");
        }
    }
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            HostBuilder builder = new HostBuilder();
            builder.ConfigureAppConfiguration((hbc, cb) =>
            {
                cb.SetBasePath(Directory.GetCurrentDirectory());
                cb.AddJsonFile("appsettings.json", false);
            });
            builder.ConfigureServices(sc =>
            {
            });
            builder.UseServiceProviderFactory(new AutofacServiceProviderFactory(cb =>
            {
                cb.AddRabbitMq();
                cb.AddHandlers();
            }));
            var host = builder.Build();
            //host.Services.UseRabbitMq().SubscribeEvent<TestEvent>(queueName: "test_queue");
            var pub = host.Services.GetService<IBusPublisher>();
            await pub.PublishAsync(new TestEvent { TestValue = 997 }, CorrelationContext.Empty);

            await host.RunAsync();
        }
    }
}

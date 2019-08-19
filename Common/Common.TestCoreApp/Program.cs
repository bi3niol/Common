using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common.Standard.Messages;
using Common.Standard.RabbitMQ;
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
            }));
            var host = builder.Build();
            host.Services.UseRabbitMq().SubscribeEvent<TestEvent>();
            await host.RunAsync();
        }
    }
}

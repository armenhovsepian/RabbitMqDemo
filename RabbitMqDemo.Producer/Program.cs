using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMqDemo.Producer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();

            // Application code should start here.
            RabbitMQManager rabbitMqManager = CreateInstance<RabbitMQManager>(host.Services);
            SendMessage(rabbitMqManager);

            await host.RunAsync();
        }

        private static void SendMessage(RabbitMQManager rabbitMqManager)
        {
            while (true)
            {
                string message = $"Hello from Producer ({DateTime.Now})";
                rabbitMqManager.SendMessage(message);

                Console.WriteLine(" ==> Message sent");
                Thread.Sleep(3000);
            }
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, configuration) =>
                {
                    configuration.Sources.Clear();

                    IHostEnvironment env = hostingContext.HostingEnvironment;

                    configuration
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables() // overwrite configurations with environment variables
                        .AddCommandLine(args); // overwrite configurations with cli arguments e.g: dotnet run MyKey="My key from command line" Position:Title=Cmd Position:Name=Cmd_Rick
                })
                .ConfigureServices((_, services) =>
                {
                    services.AddTransient<RabbitMQManager>();
                });


        static T CreateInstance<T>(IServiceProvider services)
        {
            IServiceScope serviceScope = services.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;
            return provider.GetRequiredService<T>();
        }
    }
}

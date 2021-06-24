using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace MyMediator.Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddTransient<PrintToConsoleHandler>()
                .BuildServiceProvider();

            IMediator mediator = new Mediator(serviceProvider.GetRequiredService, typeof(Program));

            var request = new PrintToConsoleRequest()
            {
                Text = "Teste was"
            };

            await mediator.SendAsync(request);
        }
    }
}

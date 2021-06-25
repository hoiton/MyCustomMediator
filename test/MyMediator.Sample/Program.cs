using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MyMediator.DependencyInjection;

namespace MyMediator.Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddTransient<PrintToConsoleHandler>()
                .AddMediator(typeof(Program))
                .BuildServiceProvider();

            var mediator = serviceProvider.GetService<IMediator>();

            var request = new PrintToConsoleRequest()
            {
                Text = "Teste was"
            };

            await mediator.SendAsync(request);
        }
    }
}

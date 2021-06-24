using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MyMediator.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMediator(this IServiceCollection services, Type[] markers)
        {
            var allHandlers =
                markers.SelectMany(m => GetTypesImplementingInterface(m.Assembly, typeof(IRequestHandler<>))).ToList();

            var serviceDescriptors = 
                allHandlers.Select(h => new ServiceDescriptor(h, h, ServiceLifetime.Transient));

            services.TryAdd(serviceDescriptors);

            services.AddSingleton<IMediator>(x => new Mediator(x.GetRequiredService, allHandlers));

            return services;
        }

        private static IEnumerable<Type> GetTypesImplementingInterface(Assembly assembly, Type interfaceType)
        {
            return assembly.ExportedTypes
                .Where(t =>
                    !t.IsInterface
                    && !t.IsAbstract
                    && t.GetInterfaces().Any(i =>
                        i.IsGenericType && interfaceType.IsAssignableFrom(i.GetGenericTypeDefinition())));
        }
    }
}

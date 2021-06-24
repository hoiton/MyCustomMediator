using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
namespace MyMediator
{
    public class Mediator : IMediator
    {
        private readonly Func<Type, object> _serviceResolver;
        private readonly ConcurrentDictionary<Type, Type> _requestToHandlerDictionary = 
            new ConcurrentDictionary<Type, Type>();

        private readonly List<Type> _allHandlerTypes;

        public Mediator(Func<Type, object> serviceResolver, Type startupType)
        {
            _serviceResolver = serviceResolver;

            var requestHandlerInterface = typeof(IRequestHandler<,>);

            _allHandlerTypes = startupType.Assembly.GetTypes()
                .Where(t =>
                    !t.IsInterface
                    && !t.IsAbstract
                    & t.GetInterfaces().Any(i =>
                        i.IsGenericType && requestHandlerInterface.IsAssignableFrom(i.GetGenericTypeDefinition())))
                .ToList();
        }

        public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));
            
            var requestType = request.GetType();

            var handlerType = _requestToHandlerDictionary.GetOrAdd(requestType,
                _allHandlerTypes
                    .FirstOrDefault(t => t.GetInterfaces().Any(i => i.IsGenericType
                                                                    && i.GetGenericArguments()[0] == requestType)));

            if (handlerType is null)
                throw new ArgumentException("For the given request doesn't exist a handler", nameof(request));
            

            var method = handlerType.GetMethod("Handle", new []{request.GetType(), typeof(CancellationToken)});
            var handler = _serviceResolver(handlerType);

            var awaitable = (Task<TResponse>)method?.Invoke(handler, new object[] {request, cancellationToken});
            await awaitable;
            return awaitable.GetAwaiter().GetResult();
        }
    }
}

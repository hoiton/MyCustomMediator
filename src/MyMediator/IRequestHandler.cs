using System.Threading;
using System.Threading.Tasks;

namespace MyMediator
{
    public interface IRequestHandler<in TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default);
    }

    public interface IRequestHandler<in TRequest> : IRequestHandler<TRequest, Unit>
        where TRequest : IRequest<Unit> { }

    public abstract class AsyncRequestHandler<TRequest> : IRequestHandler<TRequest>
        where TRequest : IRequest<Unit>
    {
        async Task<Unit> IRequestHandler<TRequest,Unit>.Handle(TRequest request, CancellationToken cancellationToken = default)
        {
            await Handle(request, cancellationToken).ConfigureAwait(false);
            return Unit.Value;
        }

        public abstract Task Handle(TRequest request, CancellationToken cancellationToken = default);
    }

    public abstract class RequestHandler<TRequest> : IRequestHandler<TRequest>
        where TRequest : IRequest<Unit>
    {
        public Task<Unit> Handle(TRequest request, CancellationToken _)
        {
            Handle(request);
            return Unit.Task;
        }

        public abstract void Handle(TRequest request);
    }

    public abstract class AsyncRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default);
    }

    public abstract class RequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public Task<TResponse> Handle(TRequest request, CancellationToken _)
        {
            return Task.FromResult(Handle(request));
        }

        public abstract TResponse Handle(TRequest request);
    }

}

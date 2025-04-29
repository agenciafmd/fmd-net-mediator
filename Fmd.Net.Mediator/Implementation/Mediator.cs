using Fmd.Net.Mediator.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Fmd.Net.Mediator.Implementation;

public class Mediator(IServiceProvider provider) : IMediator
{
    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, 
        CancellationToken cancellationToken = default)
    {
        using var scope = provider.CreateScope();
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        var handler = scope.ServiceProvider.GetRequiredService(handlerType);
        if (handler == null)
        {
            throw new InvalidOperationException($"Handler não encontrado para {request.GetType().Name}");
        }

        return await (Task<TResponse>)handlerType
            .GetMethod("Handle")!
            .Invoke(handler, [request, cancellationToken])!;
    }

    public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) 
        where TNotification : INotification
    {
        using var scope = provider.CreateScope();
        var handlerType = typeof(INotificationHandler<>).MakeGenericType(notification.GetType());
        var handlers = scope.ServiceProvider.GetServices(handlerType);

        foreach (var handler in handlers)
        {
            await (Task)handlerType
                .GetMethod("Handle")!
                .Invoke(handler, [notification, cancellationToken])!;
        }
    }
}
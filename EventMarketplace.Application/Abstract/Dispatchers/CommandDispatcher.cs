using Microsoft.Extensions.DependencyInjection;

namespace EventMarketplace.Application.Abstract.Dispatchers;

public class CommandDispatcher(IServiceProvider provider) : ICommandDispatcher
{
    public async Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken) where TCommand : ICommand
    {
        var handler = provider.GetRequiredService<ICommandHandler<TCommand>>();
        await handler.ExecuteHandleAsync(command, cancellationToken);
    }

    public async Task<TResult> SendAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand<TResult>
    {
        var handler = provider.GetRequiredService<ICommandHandler<TCommand, TResult>>();
        return await handler.ExecuteHandleAsync(command, cancellationToken);
    }
}
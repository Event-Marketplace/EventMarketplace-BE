using Microsoft.Extensions.DependencyInjection;

namespace EventMarketplace.Application.Abstract.Dispatchers;

public class QueryDispatcher(IServiceProvider provider) : IQueryDispatcher
{
    public async Task<TResult> QueryAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken) where TQuery : IQuery<TResult>
    {
        var handler = provider.GetRequiredService<IQueryHandler<TQuery, TResult>>();
        return await handler.ExecuteHandleAsync(query, cancellationToken);
    }
}
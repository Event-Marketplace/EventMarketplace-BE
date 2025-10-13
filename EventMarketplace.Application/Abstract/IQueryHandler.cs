namespace EventMarketplace.Application.Abstract;

public interface IQueryHandler<in TQuery, TResult> where TQuery: IQuery<TResult>
{
    Task<TResult> ExecuteHandleAsync(TQuery query, CancellationToken cancellationToken);
}
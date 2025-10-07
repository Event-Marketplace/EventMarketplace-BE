namespace EventMarketplace.Application.Abstract;

public interface IQueryHandler<in TQuery, TResult> where TQuery: class, IQuery<TResult>
{
    Task<TResult> ExecuteHandleAsync(TQuery query);
}
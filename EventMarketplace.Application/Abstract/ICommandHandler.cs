namespace EventMarketplace.Application.Abstract;

public interface ICommandHandler<in TCommand> where TCommand: ICommand
{
    Task ExecuteHandleAsync(TCommand command, CancellationToken cancellationToken = default);
}

public interface ICommandHandler<in TCommand, TResult> where TCommand: ICommand<TResult>
{
    Task<TResult> ExecuteHandleAsync(TCommand command, CancellationToken cancellationToken = default);
}
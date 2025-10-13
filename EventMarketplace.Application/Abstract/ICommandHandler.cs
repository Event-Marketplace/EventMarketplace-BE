namespace EventMarketplace.Application.Abstract;

public interface ICommandHandler<in TCommand> where TCommand: ICommand
{
    Task ExecuteHandleAsync(TCommand command, CancellationToken cancellationToken);
}
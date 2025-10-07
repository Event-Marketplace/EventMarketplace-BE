namespace EventMarketplace.Application.Abstract;

public interface ICommandHandler<in TCommand> where TCommand: class, ICommand
{
    Task ExecuteHandleAsync(TCommand command);
}
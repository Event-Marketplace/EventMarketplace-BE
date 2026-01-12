using MediatR;

namespace EventMarketplace.Application.CqrsAbstract;

public interface ICommand : IRequest
{
    
}

public interface ICommand<out TResponse> : IRequest<TResponse>, ICommand
{
    
}

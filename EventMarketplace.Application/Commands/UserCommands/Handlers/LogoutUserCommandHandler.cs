using EventMarketplace.Application.Patterns;
using EventMarketplace.Application.Utils.Jwt;
using MediatR;

namespace EventMarketplace.Application.Commands.UserCommands.Handlers;

public class LogoutUserCommandHandler(IUnitOfWork unitOfWork, IJwtProvider jwtProvider) : IRequestHandler<LogoutUserCommand>
{
    public async Task Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        
        try
        {
            var refreshTokenFromCookies = jwtProvider.GetRefreshTokenFromCookies();
            var refreshTokenEntity = await unitOfWork.AuthRepo.GetEntityByRefreshTokenValue(refreshTokenFromCookies) 
                                     ?? throw new Exception("Brak refresh token'a w bazie danych.");

            refreshTokenEntity.SetRevokedToken();
            
            jwtProvider.SetNullRefreshTokenInCookies();
            await unitOfWork.AuthRepo.UpdateRefreshTokenEntity(refreshTokenEntity);
            
            await unitOfWork.CommitAsync(cancellationToken);
        }
        catch (Exception e)
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
using EventMarketplace.Application.Patterns;
using EventMarketplace.Application.Utils.Jwt;
using MediatR;

namespace EventMarketplace.Application.Commands.UserCommands.Handlers;

public class LogoutUserCommandHandler(IUnitOfWork unitOfWork, IJwtProvider jwtProvider) : IRequestHandler<LogoutUserCommand>
{
    public async Task Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        var refreshTokenFromCookies = jwtProvider.GetRefreshTokenFromCookies();
        var refreshTokenEntity = await unitOfWork.Auths.GetEntityByRefreshTokenValue(refreshTokenFromCookies) 
                                 ?? throw new Exception("No refresh token in database!");

        refreshTokenEntity.SetRevokedToken();
        
        jwtProvider.SetNullRefreshTokenInCookies();
        await unitOfWork.Auths.UpdateRefreshTokenEntity(refreshTokenEntity);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
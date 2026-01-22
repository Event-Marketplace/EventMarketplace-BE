using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Application.Response;
using EventMarketplace.Application.Utils.Jwt;
using EventMarketplace.Domain.Entities;
using MediatR;

namespace EventMarketplace.Application.Commands.UserCommands.Handlers;

public class RegenerateTokensCommandHandler(IUnitOfWork unitOfWork, IJwtProvider jwtProvider) : IRequestHandler<RegenerateTokensCommand,JwtTokenResponse>
{
    public async Task<JwtTokenResponse> Handle(RegenerateTokensCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = jwtProvider.GetRefreshTokenFromCookies();
        var refreshFromDb = await unitOfWork.Auths.GetEntityByRefreshTokenValue(refreshToken) 
                            ?? throw new AppException("No refresh token in database.");

        if (refreshFromDb.Expires < DateTime.UtcNow) throw new Exception("Refresh token revoked.");
        
        var loggedUser = await unitOfWork.Users.GetUserByIdAsync(refreshFromDb.UserId) 
                         ?? throw new AppException($"User with id: {refreshFromDb.UserId}, not found.");
        
        var jwtToken = jwtProvider.GenerateToken(loggedUser);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new JwtTokenResponse()
        {
            TokenJwt = jwtToken
        };
    }
}
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
                            ?? throw new EmAppException("No refresh token in database.","NO_REFRESH_TOKEN");

        if (refreshFromDb.Expires < DateTime.UtcNow) throw new EmAppException("Refresh token revoked.","REFRESH_TOKEN_REVOKED");
        
        var loggedUser = await unitOfWork.Users.GetUserByIdAsync(refreshFromDb.UserId) 
                         ?? throw new EmNotFoundException($"User with id: {refreshFromDb.UserId}, not found.");
        
        var jwtToken = jwtProvider.GenerateToken(loggedUser);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new JwtTokenResponse()
        {
            TokenJwt = jwtToken
        };
    }
}
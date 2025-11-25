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
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        
        try
        {
            var refreshToken = jwtProvider.GetRefreshTokenFromCookies();
            var refreshFromDb = await unitOfWork.AuthRepo.GetEntityByRefreshTokenValue(refreshToken) 
                                ?? throw new AppException("Brak refresh token'a w bazie danych.");
            
            var loggedUser = await unitOfWork.Users.GetUserByIdAsync(refreshFromDb.UserId) 
                             ?? throw new AppException("Brak zalogowanego użytkownika w bazie danych.");
            
            var jwtToken = jwtProvider.GenerateToken(loggedUser);
            var newRefreshToken = jwtProvider.GenerateRefreshToken(loggedUser);
            
            refreshFromDb.Revoked = true;
            refreshFromDb.RevokedAt = DateTime.UtcNow;
            
            await unitOfWork.AuthRepo.UpdateRefreshTokenEntity(refreshFromDb);
            await unitOfWork.AuthRepo.AddNewRefreshTokenAsync(new RefreshToken()
            {
                Value = newRefreshToken.RefreshToken,
                Expires = newRefreshToken.Expires,
                Revoked = false,
                CreateAt = DateTime.UtcNow,
                UserId = loggedUser.Id
            });

            jwtProvider.AppendRefreshToken(newRefreshToken.RefreshToken);
            await unitOfWork.CommitAsync(cancellationToken);

            return new JwtTokenResponse()
            {
                TokenJwt = jwtToken
            };
        }
        catch (Exception e)
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
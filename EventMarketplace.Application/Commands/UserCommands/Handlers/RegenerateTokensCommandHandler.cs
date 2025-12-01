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

            if (refreshFromDb.Expires < DateTime.UtcNow) throw new Exception("Refresh token wygasł.");
            
            var loggedUser = await unitOfWork.Users.GetUserByIdAsync(refreshFromDb.UserId) 
                             ?? throw new AppException("Brak zalogowanego użytkownika w bazie danych.");
            
            var jwtToken = jwtProvider.GenerateToken(loggedUser);
            
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
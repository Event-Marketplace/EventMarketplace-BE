
using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Application.Response;
using EventMarketplace.Application.Utils;
using EventMarketplace.Application.Utils.Jwt;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventMarketplace.Application.Commands.UserCommands.Handlers;

public class LoginUserCommandHandler(
    IUnitOfWork unitOfWork, 
    IPasswordManager passwordManager, 
    ILogger<LoginUserCommandHandler> logger,
    IJwtProvider jwtProvider
    ) : IRequestHandler<LoginUserCommand, LoginUserResponse>
{
    public async Task<LoginUserResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var userFromDb = await unitOfWork.Users.GetUserByEmailAsync(request.Dto.Email) ??
                             throw new AppException("Użytkownik o podanym mailu nie istnieje w naszej bazie.");

            if (!passwordManager.ValidPassword(request.Dto.Password, userFromDb.Password))
                throw new AppException("Podane hasło jest nieprawidłowe.");

            var jwtToken = jwtProvider.GenerateToken(userFromDb);
            var refreshToken = jwtProvider.GenerateRefreshToken(userFromDb);
            
            jwtProvider.AppendRefreshToken(refreshToken.RefreshToken);
            
            
            await unitOfWork.CommitAsync(cancellationToken);
            logger.LogInformation("Użytkownik został poprawnie zalogowany.");
            
            return new LoginUserResponse()
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
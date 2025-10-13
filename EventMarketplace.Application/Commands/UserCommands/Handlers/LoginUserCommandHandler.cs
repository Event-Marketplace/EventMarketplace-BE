using EventMarketplace.Application.Abstract;
using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Application.Response;
using EventMarketplace.Application.Utils;
using EventMarketplace.Application.Utils.Jwt;
using Microsoft.Extensions.Logging;

namespace EventMarketplace.Application.Commands.UserCommands.Handlers;

public class LoginUserCommandHandler(
    IUnitOfWork unitOfWork, 
    IPasswordManager passwordManager, 
    ILogger<LoginUserCommandHandler> logger,
    IJwtProvider jwtProvider
    ) : ICommandHandler<LoginUserCommand, LoginUserResponse>
{
    public async Task<LoginUserResponse> ExecuteHandleAsync(LoginUserCommand command, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var userFromDb = await unitOfWork.Users.GetUserByEmailAsync(command.Dto.Email) ??
                             throw new AppException("Użytkownik o podanym mailu nie istnieje w naszej bazie.");

            if (!passwordManager.ValidPassword(command.Dto.Password, userFromDb.Password))
                throw new AppException("Podane hasło jest nieprawidłowe.");
            
            await unitOfWork.CommitAsync(cancellationToken);
            logger.LogInformation("Użytkownik został poprawnie zalogowany.");

            return new LoginUserResponse()
            {
                TokenJwt = jwtProvider.GenerateToken(userFromDb)
            };
        }
        catch (Exception e)
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
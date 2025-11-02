using EventMarketplace.Application.Abstract;
using EventMarketplace.Application.Queries;
using EventMarketplace.Application.Response.UserResponse;
using EventMarketplace.Domain.Repositories;

namespace EventMarketplace.Infrastructure.DAL.QueryHandlers;

public sealed class GetUserInfoQueryHandler(IUserRepository userRepository) : IQueryHandler<GetUserInfoQuery, GetUserInfoResponse>
{
    public async Task<GetUserInfoResponse> ExecuteHandleAsync(GetUserInfoQuery query, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByEmailAsync(query.Email) ?? throw new ApplicationException("Nie znaleziono użytkownika o danym adresie e-mail");

        return new GetUserInfoResponse()
        {
            Id = user.Id,
            Email = user.EmailAddress.Value,
            FirstName = user.FullName.FirstName,
            LastName = user.FullName.LastName,
            PhoneNumber = user.PhoneNumber.PhoneValue,
            Street = user.Address.Street,
            Number = user.Address.Number,
            City = user.Address.City,
            PostalCode = user.Address.PostalCode,
            RegistrationDate = user.CreateAt
        };
    }
}
using EventMarketplace.Application.Response.UserResponse;
using EventMarketplace.Application.Responses.UserResponse;
using MediatR;

namespace EventMarketplace.Application.Queries.handlers;

public class GetUsersQueryHandler(IUserReadModel userReadModel) : IRequestHandler<GetUsersQuery, GetUsersListResponse>
{
    public async Task<GetUsersListResponse> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await userReadModel.GetUsersAsync();

        return new GetUsersListResponse()
        {
            Users = users.Select(x => new GetUserInfoResponse()
            {
                Id = x.Id,
                City = x.Address.City,
                Email = x.EmailAddress.Value,
                Number = x.Address.Number,
                Street = x.Address.Street,
                FirstName = x.FullName.FirstName,
                LastName = x.FullName.LastName,
                PhoneNumber = x.PhoneNumber.PhoneValue,
                PostalCode = x.Address.PostalCode,
                RegistrationDate = x.CreatedAt
            }).ToList()
        };
    }
}
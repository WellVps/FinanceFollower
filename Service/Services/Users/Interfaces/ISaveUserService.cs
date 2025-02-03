using Domain.Domains.Users;
using Service.Cqrs.Commands.Users.Requests;

namespace Service.Services.Users.Interfaces;

public interface ISaveUserService
{
    Task<User> SaveUser(CreateUserRequest user, CancellationToken cancellationToken = default);
}
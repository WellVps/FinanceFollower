using BaseApi.Auth.Enums;
using MediatR;

namespace Service.Cqrs.Commands.Users.Requests;

public class CreateUserRequest : IRequest<bool>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public AccessRoles Role { get; set; }
    public string Password { get; set; }
}
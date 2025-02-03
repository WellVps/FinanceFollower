using Domain.Domains.Users;
using Infraestructure.Repositories.Interfaces;
using Service.Cqrs.Commands.Users.Requests;
using Service.Services.Users.Interfaces;
using MediatR;

namespace Service.Cqrs.Commands.Users.Handlers;

public class CreateUserHandler(ISaveUserService service) : IRequestHandler<CreateUserRequest, bool>
{
    private readonly ISaveUserService _service = service;

    public async Task<bool> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var savedUser = await _service.SaveUser(request, cancellationToken);

        return savedUser != null;
    }
}

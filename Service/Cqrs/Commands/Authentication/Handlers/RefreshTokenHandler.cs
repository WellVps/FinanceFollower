using Infraestructure.Repositories.Interfaces;
using MediatR;
using Service.Cqrs.Commands.Authentication.Requests;
using Service.Cqrs.Commands.Authentication.Responses;

namespace Service.Cqrs.Commands.Authentication.Handlers;

public class RefreshTokenHandler : IRequestHandler<RefreshTokenRequest, AuthenticationResponse?>
{
    private readonly IUserRefreshTokenRepository _userRefreshTokenRepository;
    private readonly IUserRepository _userRepository;

    public RefreshTokenHandler(IUserRefreshTokenRepository userRefreshTokenRepository, IUserRepository userRepository)
    {
        _userRefreshTokenRepository = userRefreshTokenRepository;
        _userRepository = userRepository;
    }

    public async Task<AuthenticationResponse?> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        return null;
    }
}
using BaseApi.Auth.Handlers;
using Infraestructure.Repositories.Interfaces;
using Service.Cqrs.Commands.Authentication.Requests;
using Service.Cqrs.Commands.Authentication.Responses;
using Service.Services.Authentication.Interfaces;
using MediatR;

namespace Service.Cqrs.Commands.Authentication.Handlers;

public class AuthenticationHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IUserRefreshTokenRepository userRefreshTokenRepository) : BaseAuthenticationHandler(userRefreshTokenRepository), IRequestHandler<AuthenticationRequest, AuthenticationResponse?>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    public async Task<AuthenticationResponse?> Handle(AuthenticationRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetFirstOrDefault(x => x.Email == request.Email, cancellationToken);
        if(user is null)
        {
            return null;
        }

        var passwordValid = _passwordHasher.VerifyHashedPassword(user.Password, request.Password);
        if(!passwordValid)
        {
            return null;
        }

        return await CreateAuthenticationResponse(user, request.TokenConfigs, cancellationToken: cancellationToken);
    }
}
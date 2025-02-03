using MediatR;
using BaseApi.Auth.Dtos;
using System.Text.Json.Serialization;
using Service.Cqrs.Commands.Authentication.Responses;

namespace Service.Cqrs.Commands.Authentication.Requests;

public class AuthenticationRequest : IRequest<AuthenticationResponse?>
{
    public string Email { get; set; }
    public string Password { get; set; }

    [JsonIgnore]
    public TokenConfiguration TokenConfigs { get; set; }
}
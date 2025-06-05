using System.Text.Json.Serialization;
using System;
using BaseApi.Auth.Dtos;
using MediatR;
using Service.Cqrs.Commands.Authentication.Responses;

namespace Service.Cqrs.Commands.Authentication.Requests;

public class RefreshTokenRequest: IRequest<AuthenticationResponse?>
{
    public string RefreshToken { get; set; }

    [JsonIgnore]
    public TokenConfiguration TokenConfigs { get; set; }

    // public RefreshTokenRequest(string refreshToken, TokenConfiguration tokenConfigs)
    // {
    //     RefreshToken = refreshToken;
    //     TokenConfigs = tokenConfigs;
    // }
}

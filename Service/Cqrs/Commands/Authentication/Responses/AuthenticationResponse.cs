namespace Service.Cqrs.Commands.Authentication.Responses;

public class AuthenticationResponse
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpirationDate { get; set; }
}
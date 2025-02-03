namespace BaseApi.Auth.Dtos;

public class TokenConfiguration
{
    public string? Key { get; set; }
    public int ValidityToken { get; set; }
    public int ValidityRefreshToken { get; set; }
}
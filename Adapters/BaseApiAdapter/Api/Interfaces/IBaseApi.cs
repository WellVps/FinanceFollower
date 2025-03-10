namespace BaseApiAdapter.Api.Interfaces;

public interface IBaseApi
{
    Task GetIntegrationParameters(string Name);

    Task<T> Get<T>(string url);

    Task<T> Get<T>(string url, string token, string? authorizationType = null);
}
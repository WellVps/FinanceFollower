using BaseApiAdapter.Api.Interfaces;

namespace BaseApiAdapter.Api;

public abstract class BaseApi : IBaseApi
{
    public Task<T> Get<T>(string url)
    {
        throw new NotImplementedException();
    }

    public Task<T> Get<T>(string url, string token, string? authorizationType = null)
    {
        throw new NotImplementedException();
    }

    public Task GetIntegrationParameters(string Name)
    {
        throw new NotImplementedException();
    }

    public abstract Task<double> GetAssetPrice(string symbol);
}
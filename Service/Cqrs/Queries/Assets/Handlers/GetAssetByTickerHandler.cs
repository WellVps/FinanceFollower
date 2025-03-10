using Domain.Domains.Assets;
using Infraestructure.Repositories.Interfaces;
using MediatR;
using MongoDB.Driver;
using Service.Cqrs.Queries.Assets.Requests;

namespace Service.Cqrs.Queries.Assets.Handlers;

public class GetAssetByTickerHandler : IRequestHandler<GetAssetByTickerQuery, Asset>
{
    private readonly IAssetRepository _assetRepository;

    public GetAssetByTickerHandler(IAssetRepository assetRepository)
    {
        _assetRepository = assetRepository;
    }

    public async Task<Asset> Handle(GetAssetByTickerQuery request, CancellationToken cancellationToken)
    {
        var filter = new FilterDefinitionBuilder<Asset>().Eq(x => x.Id, request.Ticker);

        var asset = await _assetRepository.GetFirstOrDefault(filter, cancellationToken: cancellationToken);
        return asset ?? throw new KeyNotFoundException($"Asset with ticker '{request.Ticker}' not found.");
    }
}
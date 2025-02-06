using Domain.Domains.Assets;
using Infraestructure.Repositories.Interfaces;
using MediatR;
using MongoDB.Driver;
using Service.Cqrs.Queries.Assets.Requests;

namespace Service.Cqrs.Queries.Assets.Handlers;

public class GetAssetsHandler : IRequestHandler<GetAssetsQuery, IEnumerable<Asset>>
{
    private readonly IAssetRepository _assetRepository;

    public GetAssetsHandler(IAssetRepository assetRepository)
    {
        _assetRepository = assetRepository;
    }
    
    public async Task<IEnumerable<Asset>> Handle(GetAssetsQuery request, CancellationToken cancellationToken)
    {
        var filter = new FilterDefinitionBuilder<Asset>().Empty;

        return await _assetRepository.GetAll(filter, cancellationToken: cancellationToken);
    }
}
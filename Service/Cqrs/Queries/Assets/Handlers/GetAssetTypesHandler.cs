using Domain.Domains.Assets;
using Infraestructure.Repositories.Interfaces;
using MediatR;
using MongoDB.Driver;
using Service.Cqrs.Queries.Assets.Requests;

namespace Service.Cqrs.Queries.Assets.Handlers;

public class GetAssetTypesHandler : IRequestHandler<GetAssetTypesRequest, IEnumerable<AssetType>>
{
    private readonly IAssetRepository _assetRepository;

    public GetAssetTypesHandler(IAssetRepository assetRepository)
    {
        _assetRepository = assetRepository;
    }

    public async Task<IEnumerable<AssetType>> Handle(GetAssetTypesRequest request, CancellationToken cancellationToken)
    {
        var filter = new FilterDefinitionBuilder<AssetType>().Empty;
        
        return await _assetRepository.GetAll(filter, cancellationToken: cancellationToken);
    }
}
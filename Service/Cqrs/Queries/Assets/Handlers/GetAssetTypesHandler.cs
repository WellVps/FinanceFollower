using Domain.Domains.Assets;
using Infraestructure.Repositories.Interfaces;
using MediatR;
using MongoDB.Driver;
using Service.Cqrs.Queries.Assets.Requests;

namespace Service.Cqrs.Queries.Assets.Handlers;

public class GetAssetTypesHandler : IRequestHandler<GetAssetTypesQuery, IEnumerable<AssetType>>
{
    private readonly IAssetTypeRepository _assetTypeRepository;

    public GetAssetTypesHandler(IAssetTypeRepository assetTypeRepository)
    {
        _assetTypeRepository = assetTypeRepository;
    }

    public async Task<IEnumerable<AssetType>> Handle(GetAssetTypesQuery request, CancellationToken cancellationToken)
    {
        var filter = new FilterDefinitionBuilder<AssetType>().Empty;
        
        return await _assetTypeRepository.GetAll(filter, cancellationToken: cancellationToken);
    }
}
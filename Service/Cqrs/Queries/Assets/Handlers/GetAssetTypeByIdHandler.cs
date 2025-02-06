using Domain.Domains.Assets;
using Infraestructure.Repositories.Interfaces;
using MediatR;
using MongoDB.Driver;
using Service.Cqrs.Queries.Assets.Requests;

namespace Service.Cqrs.Queries.Assets.Handlers;

public class GetAssetTypeByIdHandler : IRequestHandler<GetAssetTypeByIdQuery, AssetType?>
{
    private readonly IAssetTypeRepository _assetTypeRepository;

    public GetAssetTypeByIdHandler(IAssetTypeRepository assetTypeRepository)
    {
        _assetTypeRepository = assetTypeRepository;
    }

    public async Task<AssetType?> Handle(GetAssetTypeByIdQuery request, CancellationToken cancellationToken)
    {
        var filter = new FilterDefinitionBuilder<AssetType>().Eq(x => x.Id, request.Id);
        return await _assetTypeRepository.GetFirstOrDefault(filter, cancellationToken: cancellationToken);
    }
}
using Domain.Domains.Assets;
using Infraestructure.Repositories.Interfaces;
using MediatR;
using MongoDB.Driver;
using Service.Cqrs.Queries.Assets.Requests;

namespace Service.Cqrs.Queries.Assets.Handlers;

public class GetAssetById : IRequestHandler<GetAssetByIdQuery, Asset?>
{
    private readonly IAssetRepository _assetRepository;

    public GetAssetById(IAssetRepository assetRepository)
    {
        _assetRepository = assetRepository;
    }

    public async Task<Asset?> Handle(GetAssetByIdQuery request, CancellationToken cancellationToken)
    {
        var filter = new FilterDefinitionBuilder<Asset>().Eq(x => x.Id, request.Id);

        return await _assetRepository.GetFirstOrDefault(filter,
            cancellationToken: cancellationToken);
    }
}
using Domain.Domains.Assets;
using Infraestructure.Repositories.Interfaces;
using MediatR;
using Service.Cqrs.Commands.Assets.Requests;

namespace Service.Cqrs.Commands.Assets.Handlers;

public class CreateAssetTypeHandler : IRequestHandler<CreateAssetTypeRequest, AssetType>
{
    private readonly IAssetRepository _assetTypeRepository;

    public CreateAssetTypeHandler(IAssetRepository assetTypeRepository)
    {
        _assetTypeRepository = assetTypeRepository;
    }

    public async Task<AssetType> Handle(CreateAssetTypeRequest request, CancellationToken cancellationToken)
    {
        var assetType = new AssetType(request.Description, icon: request.Icon);

        return await _assetTypeRepository.SaveAndReturn(assetType);
    }
}
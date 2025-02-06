using Domain.Domains.Assets;
using Infraestructure.Repositories.Interfaces;
using MediatR;
using Service.Cqrs.Commands.Assets.Requests;

namespace Service.Cqrs.Commands.Assets.Handlers;

public class CreateAssetTypeHandler : IRequestHandler<CreateAssetTypeCommand, AssetType>
{
    private readonly IAssetTypeRepository _assetTypeRepository;

    public CreateAssetTypeHandler(IAssetTypeRepository assetTypeRepository)
    {
        _assetTypeRepository = assetTypeRepository;
    }

    public async Task<AssetType> Handle(CreateAssetTypeCommand request, CancellationToken cancellationToken)
    {
        var assetType = new AssetType(request.Description, icon: request.Icon);

        return await _assetTypeRepository.SaveAndReturn(assetType);
    }
}
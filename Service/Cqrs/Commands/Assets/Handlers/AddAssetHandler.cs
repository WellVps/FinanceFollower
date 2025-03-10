using Domain.Domains.Assets;
using Infraestructure.Repositories.Interfaces;
using MediatR;
using Service.Cqrs.Commands.Assets.Requests;

namespace Service.Cqrs.Commands.Assets.Handlers;

public class AddAssetHandler : IRequestHandler<AddAssetCommand, Asset>
{
    private readonly IAssetRepository _assetRepository;

    public AddAssetHandler(IAssetRepository assetRepository)
    {
        _assetRepository = assetRepository;
    }

    public async Task<Asset> Handle(AddAssetCommand request, CancellationToken cancellationToken)
    {
        var asset = new Asset(request.Ticker, request.Name, request.IdAssetType, request.DataSource);

        return await _assetRepository.SaveAndReturn(asset);
    }
}
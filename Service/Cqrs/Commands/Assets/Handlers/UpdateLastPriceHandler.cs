using System.Linq.Expressions;
using Domain.Domains.Assets;
using Infraestructure.Repositories.Interfaces;
using MediatR;
using Service.Cqrs.Commands.Assets.Requests;
using Service.Cqrs.Queries.Assets.Requests;

namespace Service.Cqrs.Commands.Assets.Handlers;

public class UpdateLastPriceHandler : IRequestHandler<UpdateLastPriceCommand, bool>
{
    private readonly IMediator _mediator;
    private readonly IAssetRepository _assetRepository;

    public UpdateLastPriceHandler(IMediator mediator, IAssetRepository assetRepository)
    {
        _mediator = mediator;
        _assetRepository = assetRepository;
    }

    public async Task<bool> Handle(UpdateLastPriceCommand request, CancellationToken cancellationToken)
    {
        var asset = await _mediator.Send(new GetAssetByTickerQuery(request.Ticker), cancellationToken);
        if (asset == null)
        {
            return false;
        }

        var updates = new List<(Expression<Func<Asset, object>>, object)>
        {
            (x => x.LastPrice, request.LastPrice),
            (x => x.UpdatedAt, DateTime.UtcNow)
        };

        return await _assetRepository.Update(
            x => x.Id == asset.Id,
            cancellationToken,
            [.. updates]
        );
    }
}

using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Domain.Domains.Assets;
using Infraestructure.Repositories.Interfaces;
using MediatR;
using MongoDB.Driver;
using Service.Cqrs.Commands.Assets.Requests;
using Service.Cqrs.Queries.Assets.Requests;

namespace Service.Cqrs.Commands.Assets.Handlers;

public class UpdateAssetHandler : IRequestHandler<UpdateAssetCommand, bool>
{
    private readonly IAssetRepository  _assetRepository;
    private readonly IMediator _mediator;

    public UpdateAssetHandler(IAssetRepository assetRepository, IMediator mediator)
    {
        _assetRepository = assetRepository;
        _mediator = mediator;
    }

    public async Task<bool> Handle(UpdateAssetCommand request, CancellationToken cancellationToken)
    {
        var assetType = await _mediator.Send(new GetAssetByIdQuery(request.Id), cancellationToken);
        if (assetType == null)
            return false;

        var updates = new List<(Expression<Func<Asset, object>>, object)>();

        if (!string.IsNullOrEmpty(request.Name))
            updates.Add((x => x.Name, request.Name));

        if (!string.IsNullOrEmpty(request.Ticker))
            updates.Add((x => x.Ticker, request.Ticker));

        if (request.Active.HasValue)
            updates.Add((x => x.Active, request.Active.Value));

        if (updates.Count == 0)
            return true;

        updates.Add((x => x.UpdatedAt, DateTime.UtcNow));

        var updated = await _assetRepository.Update(
            x => x.Id == request.Id,
            cancellationToken,
            updates.ToArray()
        );

        return updated;
    }
}
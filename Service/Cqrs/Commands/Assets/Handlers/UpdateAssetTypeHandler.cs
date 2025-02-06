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

public class UpdateAssetTypeHandler : IRequestHandler<UpdateAssetTypeCommand, bool>
{
    private readonly IAssetTypeRepository  _assetTypeRepository;
    private readonly IMediator _mediator;

    public UpdateAssetTypeHandler(IAssetTypeRepository assetTypeRepository, IMediator mediator)
    {
        _assetTypeRepository = assetTypeRepository;
        _mediator = mediator;
    }

    public async Task<bool> Handle(UpdateAssetTypeCommand request, CancellationToken cancellationToken)
    {
        var assetType = await _mediator.Send(new GetAssetTypeByIdQuery(request.Id), cancellationToken);
        if (assetType == null)
            return false;

        var updates = new List<(Expression<Func<AssetType, object>>, object)>();

        if (!string.IsNullOrEmpty(request.Description))
            updates.Add((x => x.Description, request.Description));

        if (!string.IsNullOrEmpty(request.Icon))
            updates.Add((x => x.Icon, request.Icon));

        if (request.Active.HasValue)
            updates.Add((x => x.Active, request.Active.Value));

        if (updates.Count == 0)
            return true;

        updates.Add((x => x.UpdatedAt, DateTime.UtcNow));

        var updated = await _assetTypeRepository.Update(
            x => x.Id == request.Id,
            cancellationToken,
            updates.ToArray()
        );

        return updated;
    }
}
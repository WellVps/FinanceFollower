using Domain.Domains.Snapshots;
using Infraestructure.Repositories.Snapshots.Interfaces;
using MediatR;
using Service.Cqrs.Commands.Snapshots.Requests;
using Service.Cqrs.Queries.InvestmentsCqrs.Requests;

namespace Service.Cqrs.Commands.Snapshots.Handlers;

public class CreateAssetSnapshotHandler(IMediator mediator, IAssetSnapshotRepository assetSnapshotRepository) : IRequestHandler<CreateAssetSnapshotCommand>
{
    private readonly IMediator _mediator = mediator;
    private readonly IAssetSnapshotRepository _assetSnapshotRepository = assetSnapshotRepository;

    public async Task Handle(CreateAssetSnapshotCommand request, CancellationToken cancellationToken)
    {
        var investments = await _mediator.Send(new GetInvestmentsByAssetIdQuery(request.Asset.Id), cancellationToken);

        investments.ForEach(async investment => {
            var snapshot = new AssetSnapshot(
                investment.UserId,
                request.Asset.Id,
                request.Asset.LastPrice,
                investment.TotalAmount,
                investment.TotalInvested,
                investment.TotalAmount * request.Asset.LastPrice
            );

            await _assetSnapshotRepository.Save(snapshot, cancellationToken);
        });
    }
}

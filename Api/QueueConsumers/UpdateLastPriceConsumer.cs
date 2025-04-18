using BaseInfraestructure.Messaging.Interfaces;
using MediatR;
using RabbitMQCommon.Events;
using Service.Cqrs.Commands.Assets.Requests;
using Service.Cqrs.Commands.Snapshots.Requests;
using Service.Cqrs.Queries.Assets.Requests;
using YahooFinance;

namespace Api.QueueConsumers;

public class UpdateLastPriceConsumer(IMediator mediator) : IEventHandler<UpdateLastPrice>
{
    private readonly IMediator _mediator = mediator;

    public async Task<bool> Handle(UpdateLastPrice @event)
    {
        var asset = await _mediator.Send(new GetAssetByIdQuery(@event.AssetId));

        if (asset == null)
            return false;

        // TODO: Create a GetAsssetPriceFactory to get the price from different sources
        var price = await new StockData().GetAssetPrice(asset.InternalTicker);

        await _mediator.Send(new UpdateLastPriceCommand(asset.Ticker, price));

        if(@event.MakeSnapshot)
        {
            asset = await _mediator.Send(new GetAssetByIdQuery(@event.AssetId));
            await _mediator.Send(new CreateSnapshotsCommand(asset));
        }

        return true;
    }
}
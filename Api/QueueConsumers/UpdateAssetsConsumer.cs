using BaseInfraestructure.Messaging.Interfaces;
using MediatR;
using RabbitMQCommon.Constants;
using RabbitMQCommon.Events;
using Service.Cqrs.Queries.InvestmentsCqrs.Requests;

namespace Api.QueueConsumers;

public class UpdateAssetsConsumer(IMediator mediator, IEventBus eventBus) : IEventHandler<UpdateAssets>
{
    private readonly IMediator _mediator = mediator;
    private readonly IEventBus _eventBus = eventBus;

    public async Task<bool> Handle(UpdateAssets @event)
    {
        var investments = await _mediator.Send(new GetExistentInvestmentsQuery());

        var assetIdsToUpdatePrice = investments
            .GroupBy(i => i.AssetId)
            .Select(g => g.First().AssetId)
            .ToList();

        assetIdsToUpdatePrice.ForEach(assetId => _eventBus.Publish(new UpdateLastPrice(assetId, @event.MakeSnapshot), EventBusMapping.UpdateLastPrice));

        return true;
    }
}
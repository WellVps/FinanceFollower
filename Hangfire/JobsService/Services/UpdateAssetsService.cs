using BaseInfraestructure.Messaging.Interfaces;
using JobsService.Services.Interfaces;
using RabbitMQCommon.Constants;
using RabbitMQCommon.Events;

namespace JobsService.Services;

public class UpdateAssetsService(IEventBus eventBus) : IUpdateAssetsService
{
    private readonly IEventBus _eventBus = eventBus;

    public async Task UpdateAssets(bool makeSnapshot = false)
    {
        _eventBus.Publish(new UpdateAssets(makeSnapshot), EventBusMapping.UpdateAssets);
    }
}

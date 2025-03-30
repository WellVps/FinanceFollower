using Api.QueueConsumers;
using BaseApi.Extensions;
using BaseInfraestructure.Messaging.Interfaces;
using RabbitMQCommon.Constants;
using RabbitMQCommon.Events;

namespace Api.Utils;

public static class MessageBrokerConfig
{
    public static void SubscribeConsumers(IApplicationBuilder app, ConfigurationManager configuration)
    {
        var rabbitConfigurations = configuration.GetRabbitMQConfiguration();
        if(!rabbitConfigurations.IsValid || !rabbitConfigurations.Active) return;

        var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

        eventBus.Subscribe<UpdateLastPrice, UpdateLastPriceConsumer>(EventBusMapping.UpdateLastPrice);
    }
}
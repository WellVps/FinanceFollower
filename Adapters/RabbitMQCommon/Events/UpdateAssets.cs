using BaseInfraestructure.Messaging.Models;

namespace RabbitMQCommon.Events;

public record UpdateAssets(bool MakeSnapshot) : Event;
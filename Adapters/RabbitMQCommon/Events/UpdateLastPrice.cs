using BaseInfraestructure.Messaging.Models;

namespace RabbitMQCommon.Events;

/// <summary>
/// Event to update the last price of an asset
/// </summary>
/// <param name="AssetId"></param>
/// <param name="LastPrice"></param>
public record UpdateLastPrice(string AssetId, bool MakeSnapshot = false) : Event;
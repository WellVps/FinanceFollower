using BaseInfraestructure.Messaging.Models;

namespace BaseInfraestructure.Messaging.Interfaces;

public interface IEventHandler<in TEvent> : IEventHandler where TEvent : Event
{
    Task<bool> Handle(TEvent @event);
}

public interface IEventHandler;
namespace BaseInfraestructure.Messaging.Exceptions;

public class EventNameNotFoundException(string? message): Exception(message)
{
    public static EventNameNotFoundException OfEvent(string eventName) => new($"Event {eventName} not found.");
}
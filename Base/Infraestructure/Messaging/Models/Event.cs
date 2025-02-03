namespace BaseInfraestructure.Messaging.Models;

public abstract record Event
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime CreateAt { get; } = DateTime.UtcNow;
}
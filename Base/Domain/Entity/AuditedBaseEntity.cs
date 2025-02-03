using MongoDB.Bson.Serialization.Attributes;

namespace BaseDomain.Entity;

public abstract class AuditedBaseEntity : BaseEntity
{
    public DateTime CreatedAt { get; set; }
    [BsonIgnoreIfNull] public DateTime? UpdatedAt { get; set; }
}
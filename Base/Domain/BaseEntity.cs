using BaseContract;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BaseDomain;

public abstract class BaseEntity: IId
{
    [BsonIgnoreIfDefault]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonIgnore] protected NotificationDomain DomainValidation { get; set; } = new();
    protected abstract void ValidateRules();

    public NotificationDomain Validate()
    {
        DomainValidation = new NotificationDomain();
        ValidateRules();
        return DomainValidation;
    }
}
using MongoDB.Bson;
using BaseDomain.Enum;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace BaseDomain.Entity;

public abstract class StatefulAuditedBaseEntity : AuditedBaseEntity
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public RecordStatus Status { get; set; }
}
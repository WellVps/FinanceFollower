using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace BaseDomain.ValueObjects.Converters;

public class AddressCodeBsonSerializer: SerializerBase<AddressCode>
{
    public override AddressCode Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        => AddressCode.Parse(context.Reader.ReadString());

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, AddressCode value)
        => context.Writer.WriteString(value.Value);
}
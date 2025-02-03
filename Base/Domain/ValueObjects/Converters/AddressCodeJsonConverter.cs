using System.Text.Json;
using System.Text.Json.Serialization;

namespace BaseDomain.ValueObjects.Converters;

public class AddressCodeJsonConverter : JsonConverter<AddressCode>
{
    public override AddressCode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => AddressCode.Parse(reader.GetString()!);

    public override void Write(Utf8JsonWriter writer, AddressCode value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.Value);
}
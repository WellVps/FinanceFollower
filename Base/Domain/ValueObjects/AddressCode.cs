using MongoDB.Bson;
using System.Text.Json.Serialization;
using System.Diagnostics.CodeAnalysis;
using BaseDomain.ValueObjects.Converters;
using MongoDB.Bson.Serialization.Attributes;

namespace BaseDomain.ValueObjects;

[JsonConverter(typeof(AddressCodeJsonConverter))]
[BsonSerializer(typeof(AddressCodeBsonSerializer))]
public readonly record struct AddressCode(string Value) : IParsable<AddressCode>
{
    public static AddressCode GenerateNewCode() => new(ObjectId.GenerateNewId().ToString());

    public static bool TryParse(string? s, out AddressCode result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }

        result = new AddressCode(s);
        return true;
    }

    public static AddressCode Parse(string s) 
    {
        ArgumentNullException.ThrowIfNull(s);
        if (!TryParse(s, out var addressCode)) throw new FormatException();
        return addressCode;
    }

    public static AddressCode Parse(string s, IFormatProvider? provider) => Parse(s);

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out AddressCode result) => TryParse(s, out result);
}
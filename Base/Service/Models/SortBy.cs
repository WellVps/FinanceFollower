using System.Text.Json;
using System.Text.Json.Serialization;

namespace BaseService.Models;

[JsonConverter(typeof(SortByJsonConverter))]
public readonly record struct SortBy(string ColumnName, SortDirection Order) : IParsable<SortBy>
{
    public const char Separator = '|';

    public static bool TryParse(string? value, out SortBy result)
    {
        if (string.IsNullOrEmpty(value))
        {
            result = default;
            return false;
        }

        var parts = value.Split(Separator);
        if (parts.Length == 1)
        {
            result = new SortBy(parts[0], SortDirection.Asc);
            return true;
        }

        if (parts.Length > 2)
        {
            result = default;
            return false;
        }

        var sortDir = string.Equals(parts[1], "desc", StringComparison.InvariantCultureIgnoreCase)
            ? SortDirection.Desc
            : SortDirection.Asc;
        result = new SortBy(parts[0], sortDir);
        return true;
    }

    public static SortBy Parse(string s, IFormatProvider? provider)
    {
        if (!TryParse(s, out var result)) throw new FormatException();
        return result;
    }

    public static bool TryParse(string? s, IFormatProvider? provider, out SortBy result)
    {
        return TryParse(s, out result);
    }
}

public enum SortDirection
{
    Asc,
    Desc
}

public class SortByJsonConverter : JsonConverter<SortBy>
{
    public override SortBy Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (!SortBy.TryParse(value, out var result))
            throw new FormatException(message: $"Cannot parse {value} as SortBy");
        
        return result;
    }

    public override void Write(Utf8JsonWriter writer, SortBy value, JsonSerializerOptions options)
    {
        var (columnName, order) = value;
        writer.WriteStringValue($"{columnName}{SortBy.Separator}{order.ToString().ToLowerInvariant()}");
    }
}
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EntityFrameworkCore.DynamoDb.Helpers;

/// <summary>
/// Represents a JSON converter that converts <see cref="DateTime"/> values to and from ISO 8601.
/// </summary>
internal sealed class DateTimeConverter : JsonConverter<DateTime>
{
    private const string Iso8601DateTimeFormat = "yyyy-MM-ddTHH:ssZ";

    /// <inheritdoc />
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        DateTime.Parse(reader.GetString()!);

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToUniversalTime().ToString(Iso8601DateTimeFormat));
    }
}
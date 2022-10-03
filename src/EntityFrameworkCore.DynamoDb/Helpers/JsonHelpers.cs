using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EntityFrameworkCore.DynamoDb.Helpers;

/// <summary>
/// Provides methods and objects used when dealing with JSON.
/// </summary>
public static class JsonHelpers
{
    /// <summary>
    /// Gets the default serialization options.
    /// </summary>
    public static readonly JsonSerializerOptions DefaultSerializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.Never,
        WriteIndented = true,
        AllowTrailingCommas = false,
        Converters =
        {
            new DateTimeConverter()
        }
    };
}
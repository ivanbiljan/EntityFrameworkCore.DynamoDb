namespace DynamoDb.Linq.Infrastructure.Interop;

/// <summary>
///     Represents data types supported in DynamoDb.
/// </summary>
public sealed class DynamoDbType
{
    /// <summary>
    ///     Represents a boolean.
    /// </summary>
    public static readonly DynamoDbType Boolean = new("B");

    /// <summary>
    ///     Represents a number.
    /// </summary>
    public static readonly DynamoDbType Number = new("N");

    /// <summary>
    ///     Represents a collection of numbers.
    /// </summary>
    public static readonly DynamoDbType NumberSet = new("NS");

    /// <summary>
    ///     Represents a string.
    /// </summary>
    public static readonly DynamoDbType String = new("S");

    /// <summary>
    ///     Represents a collection of strings.
    /// </summary>
    public static readonly DynamoDbType StringSet = new("SS");

    private readonly string _identifier;

    private DynamoDbType(string identifier)
    {
        _identifier = identifier;
    }
}
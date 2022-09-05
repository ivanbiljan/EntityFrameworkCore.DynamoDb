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

    /// <summary>
    /// Represents a null value.
    /// </summary>
    public static readonly DynamoDbType Null = new("NULL");

    private readonly string _identifier;

    private DynamoDbType(string identifier)
    {
        _identifier = identifier;
    }

    /// <summary>
    /// Returns the <see cref="DynamoDbType"/> equivalent of the specified <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>The <see cref="DynamoDbType"/> equivalent of the specified <paramref name="type"/>.</returns>
    public static DynamoDbType FromClrType(Type type)
    {
        var typeCode = Type.GetTypeCode(type);
        switch (typeCode)
        {
            case TypeCode.Empty:
                return Null;
            case TypeCode.Object:
                return String;
            case TypeCode.Boolean:
                return Boolean;
            case TypeCode.Char:
                return String;
            case TypeCode.SByte:
            case TypeCode.Byte:
            case TypeCode.Int16:
            case TypeCode.UInt16:
            case TypeCode.Int32:
            case TypeCode.UInt32:
            case TypeCode.Int64:
            case TypeCode.UInt64:
            case TypeCode.Single:
            case TypeCode.Double:
            case TypeCode.Decimal:
                return Number;
            case TypeCode.DateTime:
                return String;
            case TypeCode.String:
                return String;
            case TypeCode.DBNull:
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
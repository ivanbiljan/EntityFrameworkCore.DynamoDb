namespace EntityFrameworkCore.DynamoDb.Infrastructure.Interop;

/// <summary>
/// Specifies the types of keys for a DynamoDb table.
/// </summary>
public enum DynamoDbKeyType
{
    /// <summary>
    /// Represents the HASH key.
    /// </summary>
    Partition,
    
    /// <summary>
    /// Represents the RANGE key.
    /// </summary>
    Sort
}
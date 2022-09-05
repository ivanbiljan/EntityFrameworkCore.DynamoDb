namespace DynamoDb.Linq.Infrastructure.Interop;

public sealed class DynamoDbKeyElement
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="DynamoDbKeyElement" /> class.
    /// </summary>
    /// <param name="name">The name of the attribute that represents the key.</param>
    /// <param name="dataType">The <see cref="DynamoDbType" /> that represents the type of data this key store.</param>
    /// <param name="keyType">The <see cref="DynamoDbKeyType" />.</param>
    public DynamoDbKeyElement(string name, DynamoDbType dataType, DynamoDbKeyType keyType)
    {
        Name = name;
        DataType = dataType;
        KeyType = keyType;
    }

    public DynamoDbType DataType { get; }

    public DynamoDbKeyType KeyType { get; }
    
    public string Name { get; }
}

public sealed class DynamoDbKeySchema
{
    public DynamoDbKeySchema(DynamoDbKeyElement partitionKey, DynamoDbKeyElement? sortKey)
    {
        PartitionKey = partitionKey;
        SortKey = sortKey;
    }

    public DynamoDbKeyElement PartitionKey { get; }

    public DynamoDbKeyElement? SortKey { get; }
}
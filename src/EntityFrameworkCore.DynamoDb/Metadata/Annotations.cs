namespace EntityFrameworkCore.DynamoDb.Metadata;

/// <summary>
/// Holds EntityFramework.DynamoDb specific annotations.
/// </summary>
internal static class Annotations
{
    /// <summary>
    /// Gets a prefix used to distinguish EntityFramework.DynamoDb metadata.
    /// </summary>
    public const string DynamoDbAnnotationPrefix = "DynamoDb:";

    /// <summary>
    /// Gets the name of the annotation that stores the provisioned throughput.
    /// </summary>
    public const string ProvisionedThroughput = DynamoDbAnnotationPrefix + nameof(Amazon.DynamoDBv2.Model.ProvisionedThroughput);

    /// <summary>
    /// Gets the name of the annotation that stores the table prefix for all tables in the DB.
    /// </summary>
    public const string TablePrefix = DynamoDbAnnotationPrefix + nameof(TablePrefix);

    /// <summary>
    /// Gets the name of the annotation that stores the table name for an entity type.
    /// </summary>
    public const string TableName = DynamoDbAnnotationPrefix + nameof(TableName);

    /// <summary>
    /// Gets the name of the annotation that stores the partition key identifier.
    /// </summary>
    public const string PartitionKey = DynamoDbAnnotationPrefix + nameof(PartitionKey);

    /// <summary>
    /// Gets the name of the annotation that stores the sort key identifier.
    /// </summary>
    public const string SortKey = DynamoDbAnnotationPrefix + nameof(SortKey);

    /// <summary>
    /// Gets the name of the annotation that stores the DynamoDb attribute name of an entity property.
    /// </summary>
    public const string DynamoDbAttribute = DynamoDbAnnotationPrefix + nameof(DynamoDbAttribute);
}
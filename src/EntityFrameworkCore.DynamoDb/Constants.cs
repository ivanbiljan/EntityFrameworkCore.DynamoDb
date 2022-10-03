namespace EntityFrameworkCore.DynamoDb;

/// <summary>
/// Defines constants for various parts of the system.
/// </summary>
public static class Constants
{
    /// <summary>
    /// Defines constants relating to the underlying DynamoDb implementation.
    /// </summary>
    public static class Dynamo
    {
        /// <summary>
        /// Gets the default store name for a property that represents the partition key in a CLR model.
        /// </summary>
        public const string DefaultPartitionKeyAttributeName = "PartitionKey";

        /// <summary>
        /// Gets the default store name for a property that represents the sort key in a CLR model.
        /// </summary>
        public const string DefaultSortKeyAttributeName = "SortKey";
    }
}
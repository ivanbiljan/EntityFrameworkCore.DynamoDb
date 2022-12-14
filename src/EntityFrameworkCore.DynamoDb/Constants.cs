namespace EntityFrameworkCore.DynamoDb;

/// <summary>
///     Defines constants for various parts of the system.
/// </summary>
public static class Constants
{
    /// <summary>
    ///     Defines constants relating to the underlying DynamoDb implementation.
    /// </summary>
    public static class Dynamo
    {
        /// <summary>
        ///     Gets the default store name for a property that represents the partition key in a CLR model.
        /// </summary>
        public const string DefaultPartitionKeyAttributeName = "PartitionKey";

        /// <summary>
        ///     Gets the default store name for a property that represents the sort key in a CLR model.
        /// </summary>
        public const string DefaultSortKeyAttributeName = "SortKey";

        /// <summary>
        ///     Defines functions used with PartiQL.
        /// </summary>
        public static class Functions
        {
            /// <summary>
            ///     Gets the function used to check whether an attribute begins with a particular substring.
            /// </summary>
            public const string BeginsWith = "BEGINS_WITH";

            /// <summary>
            ///     Gets the function used to check whether an attribute contains a particular substring.
            /// </summary>
            public const string Contains = "CONTAINS";
        }
    }
}
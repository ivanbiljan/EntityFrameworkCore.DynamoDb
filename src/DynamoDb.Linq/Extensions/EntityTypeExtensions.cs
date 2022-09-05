using Amazon.DynamoDBv2.Model;
using DynamoDb.Linq.Metadata;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DynamoDb.Linq.Extensions;

public static class EntityTypeExtensions
{
    /// <summary>
    /// Gets the name of the DynamoDb table the given <see cref="IReadOnlyEntityType"/> maps to.
    /// </summary>
    /// <param name="entityType">The entity type.</param>
    /// <returns>The name of the table if configured; otherwise, <see langword="null"/>.</returns>
    public static string? GetTableName(this IReadOnlyEntityType entityType)
    {
        while (true)
        {
            if (entityType.BaseType is null)
            {
                return (string?)entityType[Annotations.TableName];
            }

            entityType = entityType.GetRootType();
        }
    }

    /// <summary>
    /// Sets the name of the DynamoDb table the given <see cref="IMutableEntityType"/> maps to.
    /// </summary>
    /// <param name="entityType">The entity type.</param>
    /// <param name="name">The name of the table the type maps to.</param>
    /// <exception cref="ArgumentException"><paramref name="name"/> is empty.</exception>
    public static void SetTableName(this IMutableEntityType entityType, string name)
    {
        if (name.Length == 0)
        {
            throw new ArgumentException("Table name must not be empty");
        }

        entityType.SetOrRemoveAnnotation(Annotations.TableName, name);
    }

    public static ProvisionedThroughput? GetProvisionedThroughput(this IReadOnlyEntityType entityType)
    {
        // GetRootType will return the current instance if BaseType is null
        return entityType.GetRootType()[Annotations.ProvisionedThroughput] as ProvisionedThroughput;
    }
    
    /// <summary>
    ///     Returns the name of the property that is used to store the partition key.
    /// </summary>
    /// <param name="entityType">The entity type to get the partition key property name for.</param>
    /// <returns>The name of the partition key property.</returns>
    public static string? GetPartitionKeyPropertyName(this IReadOnlyEntityType entityType)
        => entityType[Annotations.PartitionKey] as string;

    /// <summary>
    ///     Sets the name of the property that is used to store the partition key.
    /// </summary>
    /// <param name="entityType">The entity type to set the partition key property name for.</param>
    /// <param name="name">The name to set.</param>
    public static void SetPartitionKeyPropertyName(this IMutableEntityType entityType, string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Partition key must not be empty");
        }

        entityType.SetOrRemoveAnnotation(Annotations.PartitionKey, name);
    }
    
    /// <summary>
    ///     Returns the name of the property that is used to store the sort key.
    /// </summary>
    /// <param name="entityType">The entity type to get the sort key property name for.</param>
    /// <returns>The name of the sort key property.</returns>
    public static string? GetSortKeyPropertyName(this IReadOnlyEntityType entityType)
        => entityType[Annotations.PartitionKey] as string;

    /// <summary>
    ///     Sets the name of the property that is used to store the sort key.
    /// </summary>
    /// <param name="entityType">The entity type to set the sort key property name for.</param>
    /// <param name="name">The name to set.</param>
    public static void SetSortKeyPropertyName(this IMutableEntityType entityType, string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Sort key must not be empty");
        }

        entityType.SetOrRemoveAnnotation(Annotations.PartitionKey, name);
    }

    public static void SetProvisionedThroughput(this IMutableEntityType entityType, int readUnits, int writeUnits)
    {
        var settings = new ProvisionedThroughput(readUnits, writeUnits);
        entityType.SetAnnotation(Annotations.ProvisionedThroughput, settings);
    }

    /// <summary>
    /// Gets the name of the DynamoDb attribute the specified <paramref name="property"/> maps to.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <returns>The name of the DynamoDb attribute the property maps to.</returns>
    public static string GetDynamoDbAttributeName(this IProperty property)
    {
        return property[Annotations.DynamoDbAttribute] as string ?? property.Name;
    }
    
    /// <summary>
    /// Sets the name of the DynamoDb attribute the specified <paramref name="property"/> maps to.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    public static void SetDynamoDbAttributeName(this IMutableProperty property, string attributeName) => property.SetAnnotation(Annotations.DynamoDbAttribute, attributeName);

    /// <summary>
    /// Determines whether the given <see cref="IEntityType"/> is a document root. Document roots are types that have no owner.
    /// </summary>
    /// <param name="entityType">The entity type.</param>
    /// <returns><see langword="true"/> if the type is a document root; otherwise, <see langword="false"/>.</returns>
    /// <seealso cref="!:https://docs.microsoft.com/en-us/ef/core/modeling/owned-entities"/>
    public static bool IsDocumentRoot(this IReadOnlyEntityType entityType)
    {
        while (true)
        {
            if (entityType.BaseType is null)
            {
                return entityType[Annotations.TableName] is not null || entityType.FindOwnership() is null;
            }

            entityType = entityType.BaseType;
        }
    }
}
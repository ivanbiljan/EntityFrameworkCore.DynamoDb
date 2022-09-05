using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamoDb.Linq.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="EntityTypeBuilder"/> type.
/// </summary>
[PublicAPI]
public static class EntityTypeBuilderExtensions
{
    /// <summary>
    /// Configures the DynamoDb table name the entity being configured maps to.
    /// </summary>
    /// <param name="entityTypeBuilder">The entity builder.</param>
    /// <param name="tableName">The table the entity maps to.</param>
    /// <returns>The modified <see cref="EntityTypeBuilder"/>.</returns>
    /// <exception cref="ArgumentException"><paramref name="tableName"/> is empty.</exception>
    public static EntityTypeBuilder ToDynamoDbTable(this EntityTypeBuilder entityTypeBuilder, string tableName)
    {
        if (string.IsNullOrWhiteSpace(tableName))
        {
            throw new ArgumentException("Table name must not be empty", nameof(tableName));
        }

        entityTypeBuilder.Metadata.SetTableName(tableName);

        return entityTypeBuilder;
    }

    /// <summary>
    /// Configures the DynamoDb table name the entity being configured maps to.
    /// </summary>
    /// <param name="entityTypeBuilder">The entity builder.</param>
    /// <param name="tableName">The table the entity maps to.</param>
    /// <returns>The modified <see cref="EntityTypeBuilder"/>.</returns>
    /// <exception cref="ArgumentException"><paramref name="tableName"/> is empty.</exception>
    public static EntityTypeBuilder<TEntity> ToDynamoDbTable<TEntity>(
        this EntityTypeBuilder<TEntity> entityTypeBuilder,
        string tableName) where TEntity : class =>
        (EntityTypeBuilder<TEntity>)ToDynamoDbTable((EntityTypeBuilder)entityTypeBuilder, tableName);
}
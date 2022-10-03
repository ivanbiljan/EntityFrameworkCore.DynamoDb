using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFrameworkCore.DynamoDb.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="PropertyBuilder"/> type.
/// </summary>
[PublicAPI]
public static class PropertyBuilderExtensions
{
    /// <summary>
    /// Configures the DynamoDb attribute name the property being configured maps to.
    /// </summary>
    /// <param name="propertyBuilder">The property builder.</param>
    /// <param name="attributeName">The attribute the property maps to.</param>
    /// <returns>The modified <see cref="PropertyBuilder"/>.</returns>
    /// <exception cref="ArgumentException"><paramref name="attributeName"/> is empty.</exception>
    public static PropertyBuilder ToDynamoDbAttribute(this PropertyBuilder propertyBuilder, string attributeName)
    {
        if (string.IsNullOrWhiteSpace(attributeName))
        {
            throw new ArgumentException("Attribute name must not be empty", nameof(attributeName));
        }

        propertyBuilder.Metadata.SetDynamoDbAttributeName(attributeName);

        return propertyBuilder;
    }

    /// <summary>
    /// Configures the DynamoDb attribute name the property being configured maps to.
    /// </summary>
    /// <param name="propertyBuilder">The property builder.</param>
    /// <param name="attributeName">The attribute the property maps to.</param>
    /// <returns>The modified <see cref="PropertyBuilder"/>.</returns>
    /// <exception cref="ArgumentException"><paramref name="attributeName"/> is empty.</exception>
    public static PropertyBuilder<TProperty> ToDynamoDbAttribute<TProperty>(
        this PropertyBuilder<TProperty> propertyBuilder,
        string attributeName) =>
        (PropertyBuilder<TProperty>)ToDynamoDbAttribute((PropertyBuilder)propertyBuilder, attributeName);
}
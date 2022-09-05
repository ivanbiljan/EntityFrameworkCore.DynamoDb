using DynamoDb.Linq.Metadata;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DynamoDb.Linq.Extensions;

public static class PropertyExtensions
{
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
}
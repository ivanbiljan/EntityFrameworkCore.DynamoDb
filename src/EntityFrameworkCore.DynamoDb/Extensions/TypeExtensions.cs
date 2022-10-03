namespace EntityFrameworkCore.DynamoDb.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="Type"/> type.
/// </summary>
internal static class TypeExtensions
{
    /// <summary>
    /// Checks whether the provided type is considered a primitive.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns><see langword="true"/> if the type is a primitive; otherwise, <see langword="false"/>.</returns>
    /// <remarks>Primitives imply value types and strings.</remarks>
    public static bool IsPrimitive(this Type type) => type.IsValueType || type == typeof(string);
}
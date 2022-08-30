using DynamoDb.Linq.Metadata;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DynamoDb.Linq.Extensions;

public static class EntityTypeExtensions
{
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

    public static void SetTableName(this IMutableEntityType entityType, string name)
    {
        if (name.Length == 0)
        {
            throw new ArgumentException($"Table name must not be empty");
        }

        entityType.SetOrRemoveAnnotation(Annotations.TableName, name);
    }
}
using Amazon.DynamoDBv2.Model;
using DynamoDb.Linq.Metadata;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DynamoDb.Linq.Extensions;

internal static class ModelExtensions
{
    public static ProvisionedThroughput? GetProvisionedThroughput(this IReadOnlyModel model) =>
        (ProvisionedThroughput?)model[Annotations.ProvisionedThroughput];

    public static void SetProvisionedThroughput(this IMutableModel model, int readUnits, int writeUnits)
    {
        var settings = new ProvisionedThroughput(readUnits, writeUnits);
        model.SetAnnotation(Annotations.ProvisionedThroughput, settings);
    }
}
using System.Text.Json;
using System.Text.Json.Nodes;
using Amazon.DynamoDBv2.DocumentModel;
using DynamoDb.Linq.Extensions;
using DynamoDb.Linq.Helpers;
using DynamoDb.Linq.Infrastructure.Interop;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;

namespace DynamoDb.Linq;

internal sealed class DynamoDbDatabase : Database
{
    private readonly IDynamoDbClientWrapper _dynamoDbClientWrapper;
    
    public DynamoDbDatabase(DatabaseDependencies dependencies, IDynamoDbClientWrapper dynamoDbClientWrapper) : base(dependencies)
    {
        _dynamoDbClientWrapper = dynamoDbClientWrapper;
    }

    public override int SaveChanges(IList<IUpdateEntry> entries)
    {
        return entries.Count(Save);
    }

    public override async Task<int> SaveChangesAsync(
        IList<IUpdateEntry> entries,
        CancellationToken cancellationToken = default)
    {
        var affectedRows = 0;

        foreach (var entry in entries)
        {
            if (await SaveAsync(entry))
            {
                ++affectedRows;
            }
        }

        return affectedRows;
    }

    private bool Save(IUpdateEntry entry)
    {
        switch (entry.EntityState)
        {
            case EntityState.Detached:
                break;
            case EntityState.Unchanged:
                break;
            case EntityState.Deleted:
            {
                return _dynamoDbClientWrapper.DeleteDocument(entry);
            }
            case EntityState.Modified:
            {
                var entryAsDocument = ConvertEntryToDynamoDocument(entry);

                return _dynamoDbClientWrapper.Upsert(entry.EntityType.GetTableName(), entryAsDocument);
            }
            case EntityState.Added:
            {
                var entryAsDocument = ConvertEntryToDynamoDocument(entry);

                return _dynamoDbClientWrapper.Upsert(entry.EntityType.GetTableName(), entryAsDocument);
            }
            default:
                return false;
        }

        return false;
    }
    
    private async Task<bool> SaveAsync(IUpdateEntry entry)
    {
        switch (entry.EntityState)
        {
            case EntityState.Detached:
                break;
            case EntityState.Unchanged:
                break;
            case EntityState.Deleted:
            {
                return await _dynamoDbClientWrapper.DeleteDocumentAsync(entry);
            }
            case EntityState.Modified:
            {
                var entryAsDocument = ConvertEntryToDynamoDocument(entry);

                return await _dynamoDbClientWrapper.UpsertAsync(entry.EntityType.GetTableName(), entryAsDocument);
            }
            case EntityState.Added:
            {
                var entryAsDocument = ConvertEntryToDynamoDocument(entry);

                return await _dynamoDbClientWrapper.UpsertAsync(entry.EntityType.GetTableName(), entryAsDocument);
            }
            default:
                return false;
        }

        return false;
    }

    private static Document ConvertEntryToDynamoDocument(IUpdateEntry updateEntry)
    {
        var jsonObject = new JsonObject();
        var entityType = updateEntry.EntityType;
        foreach (var property in entityType.GetProperties())
        {
            var attributeName = property.GetDynamoDbAttributeName();
            var attributeValue = updateEntry.GetCurrentProviderValue(property);

            jsonObject[attributeName] = attributeValue is null
                ? null
                : JsonSerializer.SerializeToNode(attributeValue, JsonHelpers.DefaultSerializerOptions)!.AsValue();
        }

        return Document.FromJson(jsonObject.ToJsonString());
    }
}
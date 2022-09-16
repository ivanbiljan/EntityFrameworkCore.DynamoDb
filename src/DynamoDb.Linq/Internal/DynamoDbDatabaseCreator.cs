using System.Linq.Expressions;
using Amazon.DynamoDBv2.Model;
using DynamoDb.Linq.Extensions;
using DynamoDb.Linq.Infrastructure.Interop;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;

namespace DynamoDb.Linq;

internal sealed class DynamoTableDescriptor
{
    public DynamoTableDescriptor(string name, DynamoDbKeyElement partitionKey, DynamoDbKeyElement? sortKey, ProvisionedThroughput? throughput)
    {
        Name = name;
        PartitionKey = partitionKey;
        SortKey = sortKey;
        Throughput = throughput;
    }

    public string Name { get; }
    public DynamoDbKeyElement PartitionKey { get; }
    public DynamoDbKeyElement? SortKey { get; }
    public ProvisionedThroughput? Throughput { get; }
}

[UsedImplicitly]
internal sealed class DynamoDbDatabaseCreator : IDatabaseCreator
{
    private readonly IDatabase _database;
    private readonly IUpdateAdapterFactory _updateAdapterFactory;
    private readonly IDesignTimeModel _designTimeModel;
    private readonly IDynamoDbClientWrapper _dynamoDbClientWrapper;

    public DynamoDbDatabaseCreator(
        IDynamoDbClientWrapper dynamoDbClientWrapper,
        IDesignTimeModel designTimeModel,
        IDatabase database,
        IUpdateAdapterFactory updateAdapterFactory)
    {
        _dynamoDbClientWrapper = dynamoDbClientWrapper;
        _designTimeModel = designTimeModel;
        _database = database;
        _updateAdapterFactory = updateAdapterFactory;
    }

    public bool CanConnect() => throw new NotSupportedException();

    public bool EnsureCreated()
    {
        var model = _designTimeModel.Model;

        var created = false;
        foreach (var table in GetTables(model))
        {
            var keySchema = new DynamoDbKeySchema(table.PartitionKey, table.SortKey);
            created |= _dynamoDbClientWrapper.CreateTable(table.Name, keySchema, table.Throughput);
        }
        
        Seed();

        return created;
    }

    public bool EnsureDeleted() => throw new NotSupportedException();

    public Task<bool> CanConnectAsync(CancellationToken cancellationToken = new()) => throw new NotSupportedException();

    public async Task<bool> EnsureCreatedAsync(CancellationToken cancellationToken = new())
    {
        var model = _designTimeModel.Model;

        var created = false;
        foreach (var table in GetTables(model))
        {
            var keySchema = new DynamoDbKeySchema(table.PartitionKey, table.SortKey);
            created |= await _dynamoDbClientWrapper.CreateTableAsync(table.Name, keySchema, table.Throughput, cancellationToken);
        }

        await SeedAsync(cancellationToken);

        return created;
    }

    public Task<bool> EnsureDeletedAsync(CancellationToken cancellationToken = new()) =>
        throw new NotSupportedException();

    private static IEnumerable<DynamoTableDescriptor> GetTables(IModel model)
    {
        var tableNameToEntityTypeMap = new Dictionary<string, List<IEntityType>>();
        
        // Discover all non-owned entities/document roots
        foreach (var entityType in model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            if (tableName is null)
            {
                continue;
            }

            if (!tableNameToEntityTypeMap.TryGetValue(tableName, out var entityTypes))
            {
                entityTypes = new List<IEntityType>();
                tableNameToEntityTypeMap[tableName] = entityTypes;
            }

            entityTypes.Add(entityType);
        }

        foreach (var (tableName, entityTypes) in tableNameToEntityTypeMap)
        {
            DynamoDbKeyElement? partitionKey = null;
            DynamoDbKeyElement? sortKey = null;
            ProvisionedThroughput? provisionedThroughput = null;

            foreach (var entityType in entityTypes)
            {
                partitionKey ??= GetPartitionKey(entityType);
                sortKey ??= GetSortKey(entityType);
                provisionedThroughput ??= entityType.GetProvisionedThroughput();

                yield return new DynamoTableDescriptor(
                    tableName,
                    partitionKey,
                    sortKey,
                    provisionedThroughput);
            }
        }

        DynamoDbKeyElement GetPartitionKey(IEntityType entityType)
        {
            var partitionKeyPropertyName = entityType.GetPartitionKeyPropertyName() ??
                                       Constants.Dynamo.DefaultPartitionKeyAttributeName;

            var partitionKeyProperty = entityType.FindProperty(partitionKeyPropertyName)!;
            var dynamoDbType = DynamoDbType.FromClrType(partitionKeyProperty.ClrType);

            return new DynamoDbKeyElement(
                partitionKeyProperty.GetDynamoDbAttributeName(),
                dynamoDbType,
                DynamoDbKeyType.Partition);
        }

        DynamoDbKeyElement GetSortKey(IEntityType entityType)
        {
            var sortKeyPropertyName = entityType.GetSortKeyPropertyName() ??
                                           Constants.Dynamo.DefaultSortKeyAttributeName;

            var sortKeyProperty = entityType.FindProperty(sortKeyPropertyName)!;
            var dynamoDbType = DynamoDbType.FromClrType(sortKeyProperty.ClrType);

            return new DynamoDbKeyElement(
                sortKeyProperty.GetDynamoDbAttributeName(),
                dynamoDbType,
                DynamoDbKeyType.Sort);
        }
    }

    private void Seed()
    {
        var updateAdapter = CreateStandaloneSeedUpdateAdapter();

        _database.SaveChanges(updateAdapter.GetEntriesToSave());
    }

    private async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var updateAdapter = CreateStandaloneSeedUpdateAdapter();

        await _database.SaveChangesAsync(updateAdapter.GetEntriesToSave(), cancellationToken);
    }

    private IUpdateAdapter CreateStandaloneSeedUpdateAdapter()
    {
        var updateAdapter = _updateAdapterFactory.CreateStandalone();
        foreach (var entityType in _designTimeModel.Model.GetEntityTypes())
        {
            foreach (var seedData in entityType.GetSeedData())
            {
                updateAdapter.Model.FindEntityType(entityType.Name);

                var entry = updateAdapter.CreateEntry(seedData, entityType);
                entry.EntityState = EntityState.Modified;
            }
        }

        return updateAdapter;
    }
}
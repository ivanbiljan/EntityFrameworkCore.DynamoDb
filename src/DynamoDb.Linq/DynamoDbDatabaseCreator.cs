using System.Linq.Expressions;
using Amazon.DynamoDBv2.Model;
using DynamoDb.Linq.Extensions;
using DynamoDb.Linq.Infrastructure;
using DynamoDb.Linq.Infrastructure.Interop;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;

namespace DynamoDb.Linq;

internal sealed class DynamoTableDescriptor
{
    public string Name { get; }
    public string PartitionKey { get; }
    public string? RangeKey { get; }
    public ProvisionedThroughput? Throughput { get; }

    public DynamoTableDescriptor(string name, string partitionKey, string? rangeKey, ProvisionedThroughput? throughput)
    {
        Name = name;
        PartitionKey = partitionKey;
        RangeKey = rangeKey;
        Throughput = throughput;
    }
}

public sealed class DynamoDbDatabaseCreator : IDatabaseCreator
{
    private readonly IDynamoDbClientWrapper _dynamoDbClientWrapper;
    private readonly IDesignTimeModel _designTimeModel;
    private readonly IDatabase _database;
    
    public bool CanConnect() => throw new NotImplementedException();

    public bool EnsureCreated()
    {
        return true;
    }

    private static IEnumerable<DynamoTableDescriptor> GetTables(IModel model)
    {
        var tableNameToEntityTypeMap = new Dictionary<string, List<IEntityType>>();
        foreach (var entityType in model.GetEntityTypes().Where(e => e.FindPrimaryKey() != null))
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
            
        }
    }

    public bool EnsureDeleted()
    {
        
    }

    public Task<bool> CanConnectAsync(CancellationToken cancellationToken = new()) =>
        throw new NotImplementedException();

    public Task<bool> EnsureCreatedAsync(CancellationToken cancellationToken = new()) =>
        throw new NotImplementedException();

    public Task<bool> EnsureDeletedAsync(CancellationToken cancellationToken = new()) =>
        throw new NotImplementedException();
}

public sealed class DynamoDbDatabase : IDatabase
{
    public int SaveChanges(IList<IUpdateEntry> entries) => throw new NotImplementedException();

    public Task<int> SaveChangesAsync(IList<IUpdateEntry> entries, CancellationToken cancellationToken = new CancellationToken()) => throw new NotImplementedException();

    public Func<QueryContext, TResult> CompileQuery<TResult>(Expression query, bool async) => throw new NotImplementedException();
}
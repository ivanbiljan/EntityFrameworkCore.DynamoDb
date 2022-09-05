using System.Net;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DynamoDb.Linq.Infrastructure.Interop;

/// <summary>
/// Specifies a contract for an internal wrapper around Amazon's DynamoDb SDK. This class wraps the core APIs required for DynamoDb interop.
/// </summary>
public interface IDynamoDbClientWrapper
{
    /// <summary>
    /// Creates a new table with the specified name.
    /// </summary>
    /// <param name="tableName">The name.</param>
    /// <param name="keySchema"></param>
    /// <param name="provisionedThroughput"></param>
    /// <returns><see langword="true"/> if the operation succeeds; otherwise, <see langword="false"/>.</returns>
    bool CreateTable(string tableName, DynamoDbKeySchema keySchema, ProvisionedThroughput? provisionedThroughput);

    /// <summary>
    /// Creates a new table with the specified name.
    /// </summary>
    /// <param name="tableName">The name.</param>
    /// /// <param name="keySchema"></param>
    /// <param name="provisionedThroughput"></param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> if the operation succeeds; otherwise, <see langword="false"/>.</returns>
    Task<bool> CreateTableAsync(
        string tableName,
        DynamoDbKeySchema keySchema,
        ProvisionedThroughput? provisionedThroughput,
        CancellationToken cancellationToken = default);
}

internal sealed class DynamoDbClientWrapper : IDynamoDbClientWrapper
{
    private readonly IAmazonDynamoDB _dynamoDbClient;
    private readonly IExecutionStrategy _executionStrategy;

    public DynamoDbClientWrapper(IDynamoDbSingletonOptions options, IExecutionStrategyFactory executionStrategyFactory)
    {
        var clientConfig = new AmazonDynamoDBConfig
        {
            ServiceURL = options.ServiceUrl
        };

        if (options.Region is not null)
        {
            clientConfig.RegionEndpoint = RegionEndpoint.GetBySystemName(options.Region);
        }

        _dynamoDbClient = new AmazonDynamoDBClient(options.AccessKey, options.SecretKey, clientConfig);

        _executionStrategy = executionStrategyFactory.Create();
    }

    public bool CreateTable(string tableName, DynamoDbKeySchema keySchema, ProvisionedThroughput? provisionedThroughput) 
        => CreateTableAsync(tableName, keySchema, provisionedThroughput).GetAwaiter().GetResult();

    public Task<bool> CreateTableAsync(
        string tableName,
        DynamoDbKeySchema keySchema,
        ProvisionedThroughput? provisionedThroughput,
        CancellationToken cancellationToken = default)
    {
        // TODO: figure out why this throws an NRE
        // return _executionStrategy.ExecuteAsync(
        //     (tableName, keySchema, provisionedThroughput, _dynamoDbClient),
        //     CreateTableOnceAsync,
        //     null,
        //     cancellationToken);

        return CreateTableOnceAsync(
            null,
            (tableName, keySchema, provisionedThroughput, _dynamoDbClient),
            cancellationToken);
    }

    private static async Task<bool> CreateTableOnceAsync(
        DbContext? _,
        (string tableName, DynamoDbKeySchema keySchema, ProvisionedThroughput? provisionedThroughput, IAmazonDynamoDB
            dynamoDbClient) state,
        CancellationToken cancellationToken = default)
    {
        var (tableName, keySchema, provisionedThroughput, dynamoDbClient) = state;
        var keyElements = new List<KeySchemaElement>
        {
            new(keySchema.PartitionKey.Name, KeyType.HASH),
        };

        if (keySchema.SortKey is not null)
        {
            keyElements.Add(new KeySchemaElement(keySchema.SortKey.Name, KeyType.RANGE));
        }

        var request = new CreateTableRequest(tableName, keyElements)
        {
            ProvisionedThroughput = provisionedThroughput
        };

        try
        {

            var response = await dynamoDbClient.CreateTableAsync(request, cancellationToken);

            return response.HttpStatusCode == HttpStatusCode.OK;
        }
        catch (ResourceInUseException)
        {
            return true;
        }
    }
}
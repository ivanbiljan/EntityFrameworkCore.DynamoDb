using System.Net;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
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

    /// <summary>
    /// Executes a <see cref="PutItemRequest"/> which creates or updates the provided document.
    /// </summary>
    /// <param name="tableName">The table that contains the document.</param>
    /// <param name="document">The document.</param>
    /// <returns><see langword="true"/> if the operation succeeds; otherwise, <see langword="false"/>.</returns>
    bool Upsert(string tableName, Document document);

    /// <summary>
    /// Executes a <see cref="PutItemRequest"/> which creates or updates the provided document.
    /// </summary>
    /// <param name="tableName">The table that contains the document.</param>
    /// <param name="document">The document.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> if the operation succeeds; otherwise, <see langword="false"/>.</returns>
    Task<bool> UpsertAsync(string tableName, Document document, CancellationToken cancellationToken = default);
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

    public bool Upsert(string tableName, Document document) => UpsertAsync(tableName, document).GetAwaiter().GetResult();

    public Task<bool> UpsertAsync(string tableName, Document document, CancellationToken cancellationToken = default)
    {
        // TODO: figure out why this throws an NRE
        // return _executionStrategy.ExecuteAsync(
        //     (tableName, document, _dynamoDbClient),
        //     UpsertDocumentOnceAsync,
        //     null,
        //     cancellationToken);

        return UpsertDocumentOnceAsync(null, (tableName, document, _dynamoDbClient), cancellationToken);
    }

    private static async Task<bool> UpsertDocumentOnceAsync(
        DbContext? _,
        (string tableName, Document document, IAmazonDynamoDB dynamoDbClient) state,
        CancellationToken cancellationToken = default)
    {
        var (tableName, document, dynamoDbClient) = state;
        
        var attributeMap = document.ToAttributeMap();
        var putItemRequest = new PutItemRequest
        {
            TableName = tableName,
            Item = attributeMap
        };

        var response = await dynamoDbClient.PutItemAsync(putItemRequest, cancellationToken);

        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    private static async Task<bool> CreateTableOnceAsync(
        DbContext _,
        (string tableName, DynamoDbKeySchema keySchema, ProvisionedThroughput? provisionedThroughput, IAmazonDynamoDB
            dynamoDbClient) state,
        CancellationToken cancellationToken = default)
    {
        var (tableName, keySchema, provisionedThroughput, dynamoDbClient) = state;
        var keyElements = new List<KeySchemaElement>
        {
            new(keySchema.PartitionKey.Name, KeyType.HASH),
        };

        var attributeDefinitions = new List<AttributeDefinition>
        {
            new(keySchema.PartitionKey.Name, ScalarAttributeType.S)
        };

        if (keySchema.SortKey is not null)
        {
            keyElements.Add(new KeySchemaElement(keySchema.SortKey.Name, KeyType.RANGE));
            attributeDefinitions.Add(new AttributeDefinition(keySchema.SortKey.Name, ScalarAttributeType.S));
        }

        // TODO: pick appropriate scalar type for key elements
        var request = new CreateTableRequest(tableName, keyElements)
        {
            BillingMode = provisionedThroughput is null ? BillingMode.PAY_PER_REQUEST : BillingMode.PROVISIONED,
            ProvisionedThroughput = provisionedThroughput,
            AttributeDefinitions = attributeDefinitions
        };

        try
        {

            var response = await dynamoDbClient.CreateTableAsync(request, cancellationToken);

            return response.HttpStatusCode == HttpStatusCode.OK;
        }
        catch (ResourceInUseException)
        {
            return false;
        }
    }
}
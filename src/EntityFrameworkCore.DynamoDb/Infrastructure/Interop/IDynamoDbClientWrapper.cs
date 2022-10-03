using System.Net;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using EntityFrameworkCore.DynamoDb.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;

namespace EntityFrameworkCore.DynamoDb.Infrastructure.Interop;

/// <summary>
///     Specifies a contract for an internal wrapper around Amazon's DynamoDb SDK. This class wraps the core APIs required
///     for DynamoDb interop.
/// </summary>
public interface IDynamoDbClientWrapper
{
    /// <summary>
    ///     Creates a new table with the specified name.
    /// </summary>
    /// <param name="tableName">The name.</param>
    /// <param name="keySchema"></param>
    /// <param name="provisionedThroughput"></param>
    /// <returns><see langword="true" /> if the operation succeeds; otherwise, <see langword="false" />.</returns>
    bool CreateTable(string tableName, DynamoDbKeySchema keySchema, ProvisionedThroughput? provisionedThroughput);

    /// <summary>
    ///     Creates a new table with the specified name.
    /// </summary>
    /// <param name="tableName">The name.</param>
    /// ///
    /// <param name="keySchema"></param>
    /// <param name="provisionedThroughput"></param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true" /> if the operation succeeds; otherwise, <see langword="false" />.</returns>
    Task<bool> CreateTableAsync(
        string tableName,
        DynamoDbKeySchema keySchema,
        ProvisionedThroughput? provisionedThroughput,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Executes a <see cref="DeleteItemRequest" /> which removes the provided entity from a table.
    /// </summary>
    /// <param name="updateEntry">The entry that contains the information on the entity.</param>
    /// <returns><see langword="true" /> if the operation succeeds; otherwise, <see langword="false" />.</returns>
    bool DeleteDocument(IUpdateEntry updateEntry);

    /// <summary>
    ///     Executes a <see cref="DeleteItemRequest" /> which removes the provided entity from a table.
    /// </summary>
    /// <param name="updateEntry">The entry that contains the information on the entity.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true" /> if the operation succeeds; otherwise, <see langword="false" />.</returns>
    Task<bool> DeleteDocumentAsync(IUpdateEntry updateEntry, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Executes a <see cref="PutItemRequest" /> which creates or updates the provided document.
    /// </summary>
    /// <param name="tableName">The table that contains the document.</param>
    /// <param name="document">The document.</param>
    /// <returns><see langword="true" /> if the operation succeeds; otherwise, <see langword="false" />.</returns>
    bool Upsert(string tableName, Document document);

    /// <summary>
    ///     Executes a <see cref="PutItemRequest" /> which creates or updates the provided document.
    /// </summary>
    /// <param name="tableName">The table that contains the document.</param>
    /// <param name="document">The document.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true" /> if the operation succeeds; otherwise, <see langword="false" />.</returns>
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

    public bool DeleteDocument(IUpdateEntry updateEntry) =>
        DeleteDocumentAsync(updateEntry).GetAwaiter().GetResult();

    public bool Upsert(string tableName, Document document) =>
        UpsertAsync(tableName, document).GetAwaiter().GetResult();

    public Task<bool> CreateTableAsync(
        string tableName,
        DynamoDbKeySchema keySchema,
        ProvisionedThroughput? provisionedThroughput,
        CancellationToken cancellationToken = default) =>
        _executionStrategy.ExecuteAsync(
            (tableName, keySchema, provisionedThroughput, _dynamoDbClient),
            CreateTableInternalAsync,
            null,
            cancellationToken);

    public async Task<bool> DeleteDocumentAsync(
        IUpdateEntry updateEntry,
        CancellationToken cancellationToken = default)
    {
        return await _executionStrategy.ExecuteAsync(
            (updateEntry, _dynamoDbClient),
            DeleteDocumentInternalAsync,
            null,
            cancellationToken);
    }

    public Task<bool> UpsertAsync(string tableName, Document document, CancellationToken cancellationToken = default) =>
        _executionStrategy.ExecuteAsync(
            (tableName, document, _dynamoDbClient),
            UpsertDocumentInternalAsync,
            null,
            cancellationToken);

    private static async Task<bool> CreateTableInternalAsync(
        DbContext _,
        (string tableName, DynamoDbKeySchema keySchema, ProvisionedThroughput? provisionedThroughput, IAmazonDynamoDB
            dynamoDbClient) state,
        CancellationToken cancellationToken = default)
    {
        var (tableName, keySchema, provisionedThroughput, dynamoDbClient) = state;
        var keyElements = new List<KeySchemaElement>
        {
            new(keySchema.PartitionKey.Name, KeyType.HASH)
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

    private static async Task<bool> DeleteDocumentInternalAsync(
        DbContext _,
        (IUpdateEntry updateEntry, IAmazonDynamoDB dynamoDbClient) state,
        CancellationToken cancellationToken = default)
    {
        var (updateEntry, dynamoDbClient) = state;
        var entityType = updateEntry.EntityType;
        
        var partitionKeyPropertyName = entityType.GetPartitionKeyPropertyName() ??
                                       Constants.Dynamo.DefaultPartitionKeyAttributeName;
        var partitionKeyProperty = entityType.FindProperty(partitionKeyPropertyName)!;
        
        var sortKeyPropertyName = entityType.GetSortKeyPropertyName() ?? Constants.Dynamo.DefaultSortKeyAttributeName;
        var sortKeyProperty = entityType.FindProperty(sortKeyPropertyName);
        
        var key = new Dictionary<string, AttributeValue>
        {
            [partitionKeyPropertyName] = new AttributeValue(updateEntry.GetCurrentValue(partitionKeyProperty)!.ToString())
        };

        if (sortKeyProperty is not null)
        {
            key[sortKeyPropertyName] = new AttributeValue(updateEntry.GetCurrentValue(sortKeyProperty)!.ToString());
        }

        var deleteItemRequest = new DeleteItemRequest
        {
            TableName = entityType.GetTableName(),
            Key = key
        };

        var response = await dynamoDbClient.DeleteItemAsync(deleteItemRequest, cancellationToken);

        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    private static async Task<bool> UpsertDocumentInternalAsync(
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
}
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DynamoDb.Linq.Infrastructure;

/// <summary>
/// Specifies a contract that describes DynamoDb options.
/// </summary>
public interface IDynamoDbSingletonOptions : ISingletonOptions
{
    /// <summary>
    /// Gets the AWS region.
    /// </summary>
    string? Region { get; }
    
    /// <summary>
    /// Gets the access key.
    /// </summary>
    string? AccessKey { get; }
    
    /// <summary>
    /// Gets the secret key.
    /// </summary>
    string? SecretKey { get; }
    
    /// <summary>
    /// Gets the service url.
    /// </summary>
    string? ServiceUrl { get; }
}

internal sealed class DynamoDbSingletonOptions : IDynamoDbSingletonOptions
{
    public string? Region { get; private set; }
    
    public string? AccessKey { get; private set; }
    
    public string? SecretKey { get; private set; }
    
    public string? ServiceUrl { get; private set; }
    
    public void Initialize(IDbContextOptions options)
    {
        var dynamoDbOptions = options.FindExtension<DynamoDbContextOptionsExtension>();
        if (dynamoDbOptions is null)
        {
            return;
        }

        Region = dynamoDbOptions.Region;
        AccessKey = dynamoDbOptions.AccessKey;
        SecretKey = dynamoDbOptions.SecretKey;
        ServiceUrl = dynamoDbOptions.ServiceUrl;
    }

    public void Validate(IDbContextOptions options)
    {
        var dynamoDbOptions = options.FindExtension<DynamoDbContextOptionsExtension>();
        if (dynamoDbOptions is not null &&
            dynamoDbOptions.Region != Region &&
            dynamoDbOptions.AccessKey != AccessKey &&
            dynamoDbOptions.SecretKey != SecretKey &&
            dynamoDbOptions.ServiceUrl != ServiceUrl)
        {
            throw new ArgumentException("Failed to validate options", nameof(options));
        }
    }
}
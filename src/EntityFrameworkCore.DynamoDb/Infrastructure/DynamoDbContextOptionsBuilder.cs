using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EntityFrameworkCore.DynamoDb.Infrastructure;

public sealed class DynamoDbContextOptionsBuilder
{
    private readonly DbContextOptionsBuilder _builder;

    public DynamoDbContextOptionsBuilder(DbContextOptionsBuilder builder)
    {
        _builder = builder;
    }

    /// <summary>
    /// Configures the region.
    /// </summary>
    /// <param name="region">The region.</param>
    /// <returns>The modified <see cref="DynamoDbContextOptionsBuilder"/>.</returns>
    public DynamoDbContextOptionsBuilder WithRegion(string region) => WithOption(o => o.WithRegion(region));
    
    /// <summary>
    /// Configures the service endpoint.
    /// </summary>
    /// <param name="serviceUrl">The endpoint.</param>
    /// <returns>The modified <see cref="DynamoDbContextOptionsBuilder"/>.</returns>
    public DynamoDbContextOptionsBuilder WithServiceUrl(string serviceUrl) => WithOption(o => o.WithServiceEndpoint(serviceUrl));

    /// <summary>
    /// Configures the AWS keys.
    /// </summary>
    /// <param name="accessKey">The access key.</param>
    /// <param name="secretKey">The secret key.</param>
    /// <returns>The modified <see cref="DynamoDbContextOptionsBuilder"/>.</returns>
    public DynamoDbContextOptionsBuilder WithClientSecrets(string accessKey, string secretKey) =>
        WithOption(o => o.WithClientSecrets(accessKey, secretKey));

    private DynamoDbContextOptionsBuilder WithOption(
        Func<DynamoDbContextOptionsExtension, DynamoDbContextOptionsExtension> option)
    {
        var infrastructure = (IDbContextOptionsBuilderInfrastructure)_builder;
        infrastructure.AddOrUpdateExtension(
            option(
                _builder.Options.FindExtension<DynamoDbContextOptionsExtension>() ??
                new DynamoDbContextOptionsExtension()));

        return this;
    }
}
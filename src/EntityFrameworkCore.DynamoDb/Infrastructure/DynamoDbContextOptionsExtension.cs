using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFrameworkCore.DynamoDb.Infrastructure;

/// <summary>
///     An extension of <see cref="IDbContextOptions" /> that allows for DynamoDb configuration.
/// </summary>
internal sealed class DynamoDbContextOptionsExtension : IDbContextOptionsExtension
{
    private DynamoDbContextOptionsExtensionInfo _extensionInfo;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DynamoDbContextOptionsExtension" /> class.
    /// </summary>
    public DynamoDbContextOptionsExtension()
    {
    }

    private DynamoDbContextOptionsExtension(DynamoDbContextOptionsExtension other)
    {
        Region = other.Region;
        AccessKey = other.AccessKey;
        SecretKey = other.SecretKey;
        ServiceUrl = other.ServiceUrl;
    }

    /// <summary>
    ///     Gets the access key.
    /// </summary>
    public string? AccessKey { get; private set; }

    /// <summary>
    ///     Gets the AWS region.
    /// </summary>
    public string? Region { get; private set; }

    /// <summary>
    ///     Gets the secret key.
    /// </summary>
    public string? SecretKey { get; private set; }

    /// <summary>
    ///     Gets the endpoint DynamoDb will connect to.
    /// </summary>
    public string? ServiceUrl { get; private set; }

    /// <inheritdoc />
    public DbContextOptionsExtensionInfo Info => _extensionInfo ??= new DynamoDbContextOptionsExtensionInfo(this);

    /// <inheritdoc />
    public void ApplyServices(IServiceCollection services) => services.AddEntityFrameworkDynamoDb();

    /// <inheritdoc />
    public void Validate(IDbContextOptions options)
    {
    }

    /// <summary>
    ///     Configures the AWS keys.
    /// </summary>
    /// <param name="accessKey">The access key.</param>
    /// <param name="secretKey">The secret key.</param>
    /// <returns>The modified <see cref="DynamoDbContextOptionsExtension" /> instance.</returns>
    public DynamoDbContextOptionsExtension WithClientSecrets(string accessKey, string secretKey)
    {
        var clone = Clone();

        clone.AccessKey = accessKey;
        clone.SecretKey = secretKey;

        return clone;
    }

    /// <summary>
    ///     Configures the AWS region.
    /// </summary>
    /// <param name="region">The region.</param>
    /// <returns>The modified <see cref="DynamoDbContextOptionsExtension" /> instance.</returns>
    public DynamoDbContextOptionsExtension WithRegion(string region)
    {
        var clone = Clone();

        clone.Region = region;

        return clone;
    }
    
    /// <summary>
    ///     Configures the service endpoint URL.
    /// </summary>
    /// <param name="serviceUrl">The endpoint.</param>
    /// <returns>The modified <see cref="DynamoDbContextOptionsExtension" /> instance.</returns>
    public DynamoDbContextOptionsExtension WithServiceEndpoint(string serviceUrl)
    {
        var clone = Clone();

        clone.ServiceUrl = serviceUrl;

        return clone;
    }

    private DynamoDbContextOptionsExtension Clone() => new(this);

    private sealed class DynamoDbContextOptionsExtensionInfo : DbContextOptionsExtensionInfo
    {
        private string? _logFragment;
        private int? _serviceProviderHash;

        public DynamoDbContextOptionsExtensionInfo(IDbContextOptionsExtension extension) : base(extension)
        {
        }

        private new DynamoDbContextOptionsExtension Extension =>
            (DynamoDbContextOptionsExtension)base.Extension;

        /// <inheritdoc />
        public override bool IsDatabaseProvider => true;

        /// <inheritdoc />
        public override string LogFragment
        {
            get
            {
                if (_logFragment is null)
                {
                    _logFragment = $"Region={Extension.Region}";
                }

                return _logFragment;
            }
        }

        public override int GetServiceProviderHashCode()
        {
            if (_serviceProviderHash.HasValue)
            {
                return _serviceProviderHash.Value;
            }

            var hashCode = new HashCode();

            hashCode.Add(Extension.Region);
            hashCode.Add(Extension.AccessKey);
            hashCode.Add(Extension.SecretKey);
            hashCode.Add(Extension.ServiceUrl);

            _serviceProviderHash = hashCode.ToHashCode();

            return _serviceProviderHash.Value;
        }

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {
        }

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other) =>
            other is DynamoDbContextOptionsExtensionInfo info &&
            info.Extension.Region == Extension.Region &&
            info.Extension.AccessKey == Extension.AccessKey &&
            info.Extension.SecretKey == Extension.SecretKey &&
            info.Extension.ServiceUrl == Extension.ServiceUrl;
    }
}
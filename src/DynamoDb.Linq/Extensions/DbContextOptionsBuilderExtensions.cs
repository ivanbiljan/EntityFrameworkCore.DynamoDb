using DynamoDb.Linq.Infrastructure;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DynamoDb.Linq.Extensions;

[PublicAPI]
public static class DbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder UseDynamoDb(
        this DbContextOptionsBuilder dbContextOptionsBuilder,
        string region,
        string accessKey,
        string secretKey)
    {
        var extension = dbContextOptionsBuilder.Options.FindExtension<DynamoDbContextOptionsExtension>() ??
                        new DynamoDbContextOptionsExtension();

        extension = extension.WithRegion(region).WithClientSecrets(accessKey, secretKey);
        
        ((IDbContextOptionsBuilderInfrastructure) dbContextOptionsBuilder).AddOrUpdateExtension(extension);

        return dbContextOptionsBuilder;
    }

    public static DbContextOptionsBuilder<TContext> UseDynamoDb<TContext>(
        this DbContextOptionsBuilder<TContext> dbContextOptionsBuilder,
        string region,
        string accessKey,
        string secretKey) where TContext : DbContext => (DbContextOptionsBuilder<TContext>)UseDynamoDb(
        (DbContextOptionsBuilder)dbContextOptionsBuilder,
        region,
        accessKey,
        secretKey);
    
    public static DbContextOptionsBuilder UseDynamoDb(this DbContextOptionsBuilder dbContextOptionsBuilder, string serviceUrl)
    {
        var extension = dbContextOptionsBuilder.Options.FindExtension<DynamoDbContextOptionsExtension>() ??
                        new DynamoDbContextOptionsExtension();

        extension = extension.WithServiceEndpoint(serviceUrl);
        
        ((IDbContextOptionsBuilderInfrastructure) dbContextOptionsBuilder).AddOrUpdateExtension(extension);

        return dbContextOptionsBuilder;
    }

    public static DbContextOptionsBuilder<TContext> UseDynamoDb<TContext>(
        this DbContextOptionsBuilder<TContext> dbContextOptionsBuilder,
        string serviceUrl) where TContext : DbContext => (DbContextOptionsBuilder<TContext>)UseDynamoDb(
        (DbContextOptionsBuilder)dbContextOptionsBuilder,
        serviceUrl);
}
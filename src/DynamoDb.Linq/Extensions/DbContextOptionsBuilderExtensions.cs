using DynamoDb.Linq.Infrastructure;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;

namespace DynamoDb.Linq.Extensions;

[PublicAPI]
public static class DbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder UseDynamoDb(
        this DbContextOptionsBuilder dbContextOptionsBuilder,
        string accessKey,
        string secretKey,
        Action<DynamoDbContextOptionsBuilder> options)
    {
        var extension = dbContextOptionsBuilder.Options.FindExtension<DynamoDbContextOptionsExtension>() ??
                        new DynamoDbContextOptionsExtension();

        extension = extension.WithClientSecrets(accessKey, secretKey);
        
        ((IDbContextOptionsBuilderInfrastructure) dbContextOptionsBuilder).AddOrUpdateExtension(extension);

        options(new DynamoDbContextOptionsBuilder(dbContextOptionsBuilder));

        return dbContextOptionsBuilder;
    }

    public static DbContextOptionsBuilder<TContext> UseDynamoDb<TContext>(
        this DbContextOptionsBuilder<TContext> dbContextOptionsBuilder,
        string accessKey,
        string secretKey,
        Action<DynamoDbContextOptionsBuilder> options) where TContext : DbContext
    {
        return (DbContextOptionsBuilder<TContext>)UseDynamoDb(
            (DbContextOptionsBuilder)dbContextOptionsBuilder,
            accessKey,
            secretKey,
            options);
    }
}
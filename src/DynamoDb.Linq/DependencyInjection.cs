using System.Reflection;
using DynamoDb.Linq.Infrastructure;
using DynamoDb.Linq.Infrastructure.Interop;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Extensions.DependencyInjection;

namespace DynamoDb.Linq;

public static class DependencyInjection
{
    /// <summary>
    /// Configures services required for EntityFrameworkCore.DynamoDb.
    /// </summary>
    /// <param name="serviceCollection">The <see cref="IServiceCollection"/> the services will be added to.</param>
    /// <returns>The modified <paramref name="serviceCollection"/>.</returns>
    [UsedImplicitly]
    internal static IServiceCollection AddEntityFrameworkDynamoDb(this IServiceCollection serviceCollection)
    {
        var builder = new EntityFrameworkServicesBuilder(serviceCollection);

        builder.TryAdd<IDatabaseProvider, DatabaseProvider<DynamoDbContextOptionsExtension>>()
            .TryAdd<LoggingDefinitions, DynamoDbLoggingDefinitions>()
            .TryAdd<IDatabaseCreator, DynamoDbDatabaseCreator>()
            .TryAdd<IDatabase, DynamoDbDatabase>()
            .TryAdd<IExecutionStrategyFactory, DynamoDbExecutionStrategyFactory>()
            .TryAdd<IDbContextTransactionManager, DynamoDbTransactionManager>()
            .TryAdd<IModelValidator, DynamoDbModelValidator>()
            // .TryAdd<IProviderConventionSetBuilder, DynamoDbConventionSetBuilder>()
            // .TryAdd<IValueGeneratorSelector, DynamoDbValueGeneratorSelector>()
            .TryAdd<IQueryContextFactory, DynamoDbQueryContextFactory>()
            .TryAdd<ITypeMappingSource, DynamoDbTypeMappingSource>()
            // .TryAdd<IQueryableMethodTranslatingExpressionVisitorFactory, DynamoDbQueryableMethodTranslatingExpressionVisitorFactory>()
            // .TryAdd<IShapedQueryCompilingExpressionVisitorFactory, DynamoDbShapedQueryCompilingExpressionVisitorFactory>()
            // .TryAdd<IQueryTranslationPreprocessorFactory, DynamoDbQueryTranslationPreprocessorFactory>()
            // .TryAdd<IQueryCompilationContextFactory, DynamoDbQueryCompilationContextFactory>()
            // .TryAdd<IQueryTranslationPostprocessorFactory, DynamoDbQueryTranslationPostprocessorFactory>()
            .TryAdd<ISingletonOptions, IDynamoDbSingletonOptions>(provider => provider.GetRequiredService<IDynamoDbSingletonOptions>());

        builder.TryAddProviderSpecificServices(
            map =>
            {
                map.TryAddSingleton<IDynamoDbSingletonOptions, DynamoDbSingletonOptions>();
                map.TryAddSingleton<IDynamoDbClientWrapper, DynamoDbClientWrapper>();
            });

        builder.TryAddCoreServices();

        return serviceCollection;
    }
}
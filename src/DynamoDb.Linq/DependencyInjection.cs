using System.Reflection;
using DynamoDb.Linq.Infrastructure;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
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
    public static IServiceCollection AddEntityFrameworkDynamoDb(this IServiceCollection serviceCollection)
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
            });

        builder.TryAddCoreServices();

        return serviceCollection;
    }
}

public class DynamoDbTypeMappingSource : TypeMappingSource
{
    public DynamoDbTypeMappingSource(TypeMappingSourceDependencies dependencies) : base(dependencies)
    {
    }
}

public class DynamoDbQueryContextFactory : IQueryContextFactory
{
    public QueryContext Create() => throw new NotImplementedException();
}

public class DynamoDbModelValidator : IModelValidator
{
    public void Validate(IModel model, IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
    {
        throw new NotImplementedException();
    }
}

public class DynamoDbTransactionManager : IDbContextTransactionManager
{
    public void ResetState()
    {
        throw new NotImplementedException();
    }

    public Task ResetStateAsync(CancellationToken cancellationToken = new CancellationToken()) => throw new NotImplementedException();

    public IDbContextTransaction BeginTransaction() => throw new NotImplementedException();

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = new CancellationToken()) => throw new NotImplementedException();

    public void CommitTransaction()
    {
        throw new NotImplementedException();
    }

    public Task CommitTransactionAsync(CancellationToken cancellationToken = new CancellationToken()) => throw new NotImplementedException();

    public void RollbackTransaction()
    {
        throw new NotImplementedException();
    }

    public Task RollbackTransactionAsync(CancellationToken cancellationToken = new CancellationToken()) => throw new NotImplementedException();

    public IDbContextTransaction? CurrentTransaction { get; }
}

public class DynamoDbExecutionStrategyFactory : IExecutionStrategyFactory
{
    public IExecutionStrategy Create() => throw new NotImplementedException();
}

public sealed class DynamoDbLoggingDefinitions : LoggingDefinitions
{
    
}
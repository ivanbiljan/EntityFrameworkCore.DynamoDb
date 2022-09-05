using System.Reflection;
using DynamoDb.Linq.Infrastructure;
using DynamoDb.Linq.Infrastructure.Interop;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
            // .TryAdd<IExecutionStrategyFactory, DynamoDbExecutionStrategyFactory>()
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

public class DynamoDbTypeMappingSource : TypeMappingSource
{
    public class DynamoDbTypeMapping : CoreTypeMapping
    {
        public DynamoDbTypeMapping(
            Type clrType,
            ValueComparer? comparer = null,
            ValueComparer? keyComparer = null)
            : base(
                new CoreTypeMappingParameters(
                    clrType,
                    converter: null,
                    comparer,
                    keyComparer))
        {
        }

        protected DynamoDbTypeMapping(CoreTypeMappingParameters parameters)
            : base(parameters)
        {
        }

        public override CoreTypeMapping Clone(ValueConverter? converter)
            => new DynamoDbTypeMapping(Parameters.WithComposedConverter(converter));
    }

    public DynamoDbTypeMappingSource(TypeMappingSourceDependencies dependencies) : base(dependencies)
    {
    }

    protected override CoreTypeMapping? FindMapping(in TypeMappingInfo mappingInfo)
    {
        return FindPrimitiveMapping(mappingInfo);
    }
    
    private static CoreTypeMapping? FindPrimitiveMapping(in TypeMappingInfo mappingInfo)
    {
        var clrType = mappingInfo.ClrType!;
        if ((clrType.IsValueType
             && clrType != typeof(Guid)
             && !clrType.IsEnum)
            || clrType == typeof(string))
        {
            return new DynamoDbTypeMapping(clrType);
        }

        return null;
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
    public class DynamoDbExecutionStrategy : ExecutionStrategy
    {
        public DynamoDbExecutionStrategy(DbContext context, int maxRetryCount, TimeSpan maxRetryDelay) : base(context, maxRetryCount, maxRetryDelay)
        {
        }

        public DynamoDbExecutionStrategy(ExecutionStrategyDependencies dependencies, int maxRetryCount, TimeSpan maxRetryDelay) : base(dependencies, maxRetryCount, maxRetryDelay)
        {
        }

        protected override bool ShouldRetryOn(Exception exception) => false;
    }

    private readonly ExecutionStrategyDependencies _executionStrategyDependencies;

    public DynamoDbExecutionStrategyFactory(ExecutionStrategyDependencies executionStrategyDependencies)
    {
        _executionStrategyDependencies = executionStrategyDependencies;
    }

    public IExecutionStrategy Create() =>
        new DynamoDbExecutionStrategy(_executionStrategyDependencies, 0, TimeSpan.Zero);
}

public sealed class DynamoDbLoggingDefinitions : LoggingDefinitions
{
    
}
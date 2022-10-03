using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.DynamoDb;

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
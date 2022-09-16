using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace DynamoDb.Linq;

internal class DynamoDbQueryCompilationContextFactory : IQueryCompilationContextFactory
{
    private QueryCompilationContextDependencies _dependencies;

    public DynamoDbQueryCompilationContextFactory(QueryCompilationContextDependencies dependencies)
    {
        _dependencies = dependencies;
    }

    public QueryCompilationContext Create(bool async) => new DynamoDbQueryCompilationContext(_dependencies, async);
}

internal sealed class DynamoDbQueryCompilationContext : QueryCompilationContext
{
    public DynamoDbQueryCompilationContext(QueryCompilationContextDependencies dependencies, bool async) : base(dependencies, async)
    {
    }
}
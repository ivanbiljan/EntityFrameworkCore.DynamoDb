using Microsoft.EntityFrameworkCore.Query;

namespace DynamoDb.Linq;

public class DynamoDbQueryContextFactory : IQueryContextFactory
{
    private readonly QueryContextDependencies _dependencies;

    public DynamoDbQueryContextFactory(QueryContextDependencies dependencies)
    {
        _dependencies = dependencies;
    }

    public QueryContext Create() => new DynamoDbQueryContext(_dependencies);
}

public class DynamoDbQueryContext : QueryContext
{
    public DynamoDbQueryContext(QueryContextDependencies dependencies) : base(dependencies)
    {
    }
}
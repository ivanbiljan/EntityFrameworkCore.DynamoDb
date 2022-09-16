using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace DynamoDb.Linq;

internal class DynamoDbQueryCompilationContextFactory : QueryCompilationContextFactory
{
    private QueryCompilationContextDependencies _dependencies;

    public DynamoDbQueryCompilationContextFactory(QueryCompilationContextDependencies dependencies) : base(dependencies)
    {
    }
}
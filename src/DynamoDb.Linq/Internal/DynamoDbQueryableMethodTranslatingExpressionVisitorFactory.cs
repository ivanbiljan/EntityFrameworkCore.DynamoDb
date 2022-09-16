using Microsoft.EntityFrameworkCore.Query;

namespace DynamoDb.Linq;

internal class DynamoDbQueryableMethodTranslatingExpressionVisitorFactory : IQueryableMethodTranslatingExpressionVisitorFactory
{
    private QueryableMethodTranslatingExpressionVisitorDependencies _dependencies;
    private QueryCompilationContext _queryCompilationContext;

    public QueryableMethodTranslatingExpressionVisitor Create(QueryCompilationContext queryCompilationContext)
    {
        return null;
    }
}
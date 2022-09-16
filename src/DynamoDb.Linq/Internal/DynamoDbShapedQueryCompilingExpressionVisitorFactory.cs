using Microsoft.EntityFrameworkCore.Query;

namespace DynamoDb.Linq;

internal class DynamoDbShapedQueryCompilingExpressionVisitorFactory : IShapedQueryCompilingExpressionVisitorFactory
{
    public ShapedQueryCompilingExpressionVisitor Create(QueryCompilationContext queryCompilationContext) => throw new NotImplementedException();
}
using Microsoft.EntityFrameworkCore.Query;

namespace EntityFrameworkCore.DynamoDb;

internal class DynamoDbShapedQueryCompilingExpressionVisitorFactory : IShapedQueryCompilingExpressionVisitorFactory
{
    public ShapedQueryCompilingExpressionVisitor Create(QueryCompilationContext queryCompilationContext) => throw new NotImplementedException();
}
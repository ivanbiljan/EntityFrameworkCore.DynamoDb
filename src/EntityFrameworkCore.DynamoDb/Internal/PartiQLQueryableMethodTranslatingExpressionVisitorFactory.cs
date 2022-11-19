using EntityFrameworkCore.DynamoDb.Compilation;
using Microsoft.EntityFrameworkCore.Query;

namespace EntityFrameworkCore.DynamoDb.Internal;

internal sealed class PartiQLQueryableMethodTranslatingExpressionVisitorFactory : IQueryableMethodTranslatingExpressionVisitorFactory
{
    private readonly QueryableMethodTranslatingExpressionVisitorDependencies _dependencies;
    private readonly QueryCompilationContext _queryCompilationContext;
    private readonly LinqExpressionToPartiQLTranslatingExpressionVisitor _partiQLTranslator;

    public PartiQLQueryableMethodTranslatingExpressionVisitorFactory(QueryableMethodTranslatingExpressionVisitorDependencies dependencies, QueryCompilationContext queryCompilationContext, LinqExpressionToPartiQLTranslatingExpressionVisitor partiQLTranslator)
    {
        _dependencies = dependencies;
        _queryCompilationContext = queryCompilationContext;
        _partiQLTranslator = partiQLTranslator;
    }

    /// <inheritdoc />
    public QueryableMethodTranslatingExpressionVisitor Create(QueryCompilationContext queryCompilationContext) =>
        new PartiQLQueryableMethodTranslatingExpressionVisitor(_dependencies, _queryCompilationContext, false, _partiQLTranslator);
}
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace DynamoDb.Linq;

internal class DynamoDbQueryTranslationPostprocessorFactory : IQueryTranslationPostprocessorFactory
{
    private readonly QueryTranslationPostprocessorDependencies _dependencies;

    public DynamoDbQueryTranslationPostprocessorFactory(QueryTranslationPostprocessorDependencies dependencies)
    {
        _dependencies = dependencies;
    }

    public QueryTranslationPostprocessor Create(QueryCompilationContext queryCompilationContext) =>
        new DynamoDbQueryTranslationPostprocessor(_dependencies, queryCompilationContext);
}

internal sealed class DynamoDbQueryTranslationPostprocessor : QueryTranslationPostprocessor
{
    public DynamoDbQueryTranslationPostprocessor(QueryTranslationPostprocessorDependencies dependencies, QueryCompilationContext queryCompilationContext) : base(dependencies, queryCompilationContext)
    {
    }
}
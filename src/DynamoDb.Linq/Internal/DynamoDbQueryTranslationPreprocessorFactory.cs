using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace DynamoDb.Linq;

internal class DynamoDbQueryTranslationPreprocessorFactory : IQueryTranslationPreprocessorFactory
{
    private readonly QueryTranslationPreprocessorDependencies _dependencies;

    public DynamoDbQueryTranslationPreprocessorFactory(QueryTranslationPreprocessorDependencies dependencies)
    {
        _dependencies = dependencies;
    }

    public QueryTranslationPreprocessor Create(QueryCompilationContext queryCompilationContext) =>
        new DynamoDbQueryTranslationPreprocessor(_dependencies, queryCompilationContext);
}

internal sealed class DynamoDbQueryTranslationPreprocessor : QueryTranslationPreprocessor
{
    public DynamoDbQueryTranslationPreprocessor(
        QueryTranslationPreprocessorDependencies dependencies,
        QueryCompilationContext queryCompilationContext) : base(dependencies, queryCompilationContext)
    {
    }
}
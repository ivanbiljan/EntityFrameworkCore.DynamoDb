using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace DynamoDb.Linq;

internal class DynamoDbQueryTranslationPreprocessorFactory : QueryTranslationPreprocessorFactory
{
    public DynamoDbQueryTranslationPreprocessorFactory(QueryTranslationPreprocessorDependencies dependencies) : base(dependencies)
    {
    }
}
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace DynamoDb.Linq;

internal class DynamoDbQueryTranslationPostprocessorFactory : QueryTranslationPostprocessorFactory
{
    public DynamoDbQueryTranslationPostprocessorFactory(QueryTranslationPostprocessorDependencies dependencies) : base(dependencies)
    {
    }
}
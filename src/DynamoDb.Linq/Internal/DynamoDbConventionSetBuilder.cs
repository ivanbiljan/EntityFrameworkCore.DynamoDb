using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace DynamoDb.Linq;

internal class DynamoDbConventionSetBuilder : ProviderConventionSetBuilder
{
    public DynamoDbConventionSetBuilder(ProviderConventionSetBuilderDependencies dependencies) : base(dependencies)
    {
    }
}
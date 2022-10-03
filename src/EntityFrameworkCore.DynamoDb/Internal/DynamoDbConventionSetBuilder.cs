using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace EntityFrameworkCore.DynamoDb;

internal class DynamoDbConventionSetBuilder : ProviderConventionSetBuilder
{
    public DynamoDbConventionSetBuilder(ProviderConventionSetBuilderDependencies dependencies) : base(dependencies)
    {
    }
}
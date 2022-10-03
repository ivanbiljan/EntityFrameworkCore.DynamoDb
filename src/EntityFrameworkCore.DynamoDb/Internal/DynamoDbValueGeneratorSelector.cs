using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace EntityFrameworkCore.DynamoDb;

internal class DynamoDbValueGeneratorSelector : ValueGeneratorSelector
{
    public DynamoDbValueGeneratorSelector(ValueGeneratorSelectorDependencies dependencies) : base(dependencies)
    {
    }
}
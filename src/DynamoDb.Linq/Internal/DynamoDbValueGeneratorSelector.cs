using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace DynamoDb.Linq;

internal class DynamoDbValueGeneratorSelector : ValueGeneratorSelector
{
    public DynamoDbValueGeneratorSelector(ValueGeneratorSelectorDependencies dependencies) : base(dependencies)
    {
    }
}
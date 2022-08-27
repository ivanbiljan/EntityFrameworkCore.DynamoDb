using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DynamoDb.Linq.Infrastructure;

public interface IDynamoDbSingletonOptions : ISingletonOptions
{
}

internal sealed class DynamoDbSingletonOptions : IDynamoDbSingletonOptions
{
    public void Initialize(IDbContextOptions options)
    {
        throw new NotImplementedException();
    }

    public void Validate(IDbContextOptions options)
    {
        throw new NotImplementedException();
    }
}
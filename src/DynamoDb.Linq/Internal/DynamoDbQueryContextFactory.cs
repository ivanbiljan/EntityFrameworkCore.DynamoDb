using Microsoft.EntityFrameworkCore.Query;

namespace DynamoDb.Linq;

public class DynamoDbQueryContextFactory : IQueryContextFactory
{
    public QueryContext Create() => throw new NotImplementedException();
}
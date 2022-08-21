using System.Collections;
using System.Linq.Expressions;

namespace DynamoDb.Linq;

/// <summary>
///     Defines a contract that describes
/// </summary>
public interface IDynamoDbQueryCompiler
{
}

public sealed class DynamoDbSet<TEntity> : IQueryable<TEntity>
{
    private readonly IDynamoDbQueryProvider _dynamoDbQueryProvider;

    public DynamoDbSet(IDynamoDbQueryProvider dynamoDbQueryProvider)
    {
        _dynamoDbQueryProvider = dynamoDbQueryProvider;
    }

    /// <inheritdoc />
    public Expression Expression => throw new NotSupportedException();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    public IEnumerator<TEntity> GetEnumerator() => throw new NotSupportedException();

    /// <inheritdoc />
    public IQueryProvider Provider => throw new NotSupportedException();

    /// <inheritdoc />
    public Type ElementType => typeof(TEntity);
}

/// <summary>
///     Defines a contract that defines methods to execute queries asynchronously.
/// </summary>
public interface IDynamoDbQueryProvider : IQueryProvider
{
    Task<TResult> ExecuteAsync<TResult>(Expression expression);
}

public sealed class DynamoDbQueryProvider : IDynamoDbQueryProvider
{
    public IQueryable CreateQuery(Expression expression) => throw new NotImplementedException();

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression) =>
        new DynamoDbSet<TElement>(this);

    public object? Execute(Expression expression) => throw new NotImplementedException();

    public async Task<TResult> ExecuteAsync<TResult>(Expression expression) => throw new NotImplementedException();

    public TResult Execute<TResult>(Expression expression) => throw new NotImplementedException();
}
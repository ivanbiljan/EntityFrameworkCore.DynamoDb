using Microsoft.EntityFrameworkCore.Storage;

namespace DynamoDb.Linq;

public class DynamoDbTransactionManager : IDbContextTransactionManager
{
    public void ResetState()
    {
        throw new NotImplementedException();
    }

    public Task ResetStateAsync(CancellationToken cancellationToken = new CancellationToken()) => throw new NotImplementedException();

    public IDbContextTransaction BeginTransaction() => throw new NotImplementedException();

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = new CancellationToken()) => throw new NotImplementedException();

    public void CommitTransaction()
    {
        throw new NotImplementedException();
    }

    public Task CommitTransactionAsync(CancellationToken cancellationToken = new CancellationToken()) => throw new NotImplementedException();

    public void RollbackTransaction()
    {
        throw new NotImplementedException();
    }

    public Task RollbackTransactionAsync(CancellationToken cancellationToken = new CancellationToken()) => throw new NotImplementedException();

    public IDbContextTransaction? CurrentTransaction { get; }
}
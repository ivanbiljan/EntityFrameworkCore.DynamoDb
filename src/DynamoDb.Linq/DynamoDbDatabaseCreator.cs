using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;

namespace DynamoDb.Linq;

public sealed class DynamoDbDatabaseCreator : IDatabaseCreator
{
    public bool CanConnect() => throw new NotImplementedException();

    public bool EnsureCreated() => throw new NotImplementedException();
    public bool EnsureDeleted() => throw new NotImplementedException();

    public Task<bool> CanConnectAsync(CancellationToken cancellationToken = new()) =>
        throw new NotImplementedException();

    public Task<bool> EnsureCreatedAsync(CancellationToken cancellationToken = new()) =>
        throw new NotImplementedException();

    public Task<bool> EnsureDeletedAsync(CancellationToken cancellationToken = new()) =>
        throw new NotImplementedException();
}

public sealed class DynamoDbDatabase : IDatabase
{
    public int SaveChanges(IList<IUpdateEntry> entries) => throw new NotImplementedException();

    public Task<int> SaveChangesAsync(IList<IUpdateEntry> entries, CancellationToken cancellationToken = new CancellationToken()) => throw new NotImplementedException();

    public Func<QueryContext, TResult> CompileQuery<TResult>(Expression query, bool async) => throw new NotImplementedException();
}
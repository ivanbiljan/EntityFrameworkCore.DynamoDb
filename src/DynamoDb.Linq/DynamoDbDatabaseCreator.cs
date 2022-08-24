using Microsoft.EntityFrameworkCore.Storage;

namespace DynamoDb.Linq;

public sealed class DynamoDbDatabaseCreator : IDatabaseCreator
{
    public bool EnsureDeleted() => throw new NotImplementedException();

    public Task<bool> EnsureDeletedAsync(CancellationToken cancellationToken = new CancellationToken()) => throw new NotImplementedException();

    public bool EnsureCreated() => throw new NotImplementedException();

    public Task<bool> EnsureCreatedAsync(CancellationToken cancellationToken = new CancellationToken()) => throw new NotImplementedException();

    public bool CanConnect() => throw new NotImplementedException();

    public Task<bool> CanConnectAsync(CancellationToken cancellationToken = new CancellationToken()) => throw new NotImplementedException();
}
using Microsoft.EntityFrameworkCore.Storage;

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
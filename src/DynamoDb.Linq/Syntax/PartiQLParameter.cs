using DynamoDb.Linq.Infrastructure.Interop;

namespace DynamoDb.Linq.Syntax;

/// <summary>
///     Represents a parameter in a PartiQL statement.
/// </summary>
/// <param name="Type">The <see cref="DynamoDbType" />.</param>
/// <param name="Value">The value.</param>
public sealed record PartiQLParameter(DynamoDbType Type, object? Value);
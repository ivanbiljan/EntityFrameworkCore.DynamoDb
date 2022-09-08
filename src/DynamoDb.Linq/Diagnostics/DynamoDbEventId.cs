using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace DynamoDb.Linq.Diagnostics;

/// <summary>
/// Event IDs for DynamoDb events that correspond to messages logged to an <see cref="ILogger"/> and events sent to a <see cref="DiagnosticSource"/>.
/// </summary>
/// <remarks>These IDs are also used with <see cref="WarningsConfigurationBuilder"/> to configure the behavior of warnings.</remarks>
/// <seealso cref="CoreEventId"/>
public static class DynamoDbEventId
{
    private enum Id
    {
        ExecutingCreateTable = CoreEventId.ProviderBaseId + 256,
        ExecutingDeleteTable = CoreEventId.ProviderBaseId + 257
    }

    /// <summary>
    /// CreateTable is going to be executed.
    /// </summary>
    public static EventId ExecutingCreateTable => new(
        (int)Id.ExecutingCreateTable,
        Id.ExecutingCreateTable.ToString());
    
    /// <summary>
    /// DeleteTable is going to be executed.
    /// </summary>
    public static EventId ExecutingDeleteTable => new(
        (int)Id.ExecutingCreateTable,
        Id.ExecutingCreateTable.ToString());
}
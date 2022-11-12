namespace EntityFrameworkCore.DynamoDb.Syntax.MethodTranslators;

/// <summary>
/// Defines a contract that describes modules consumers can register with Entity Framework Core to provide custom <see cref="IMethodCallTranslator"/>s.
/// </summary>
public interface IMethodCallTranslatorPlugin
{
    /// <summary>
    /// Gets an enumerable collection of translators defined by this plugin.
    /// </summary>
    IEnumerable<IMethodCallTranslator> Translators { get; }
}
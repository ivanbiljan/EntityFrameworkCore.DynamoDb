using System.Linq.Expressions;
using System.Reflection;
using EntityFrameworkCore.DynamoDb.Syntax.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EntityFrameworkCore.DynamoDb.Syntax.Internal;

/// <summary>
///     Defines a contract that describes types that translate .NET method calls (<see cref="MethodCallExpression"/>) into their DynamoDb equivalents. E.g.,
///     <c>string.Contains(string)</c> into <c>CONTAINS</c>.
/// </summary>
public interface IMethodCallTranslator
{
    /// <summary>
    ///     Takes an expression and produces the DynamoDb equivalent.
    /// </summary>
    /// <param name="source">The expression.</param>
    /// <param name="methodToTranslate">The method to translate.</param>
    /// <param name="arguments">The argument.</param>
    /// <returns>The resulting <see cref="PartiQLExpression" />.</returns>
    PartiQLExpression? Translate(
        PartiQLExpression source,
        MethodInfo methodToTranslate,
        IReadOnlyList<PartiQLExpression> arguments);
}

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

/// <summary>
/// Defines a contract for types that evaluate <see cref="MethodCallExpression"/>s against a collection of <see cref="IMethodCallTranslator"/>s.
/// </summary>
internal interface IMethodCallTranslatorProvider
{
    
    PartiQLExpression? Translate(
        IModel model,
        PartiQLExpression instance,
        MethodInfo methodToTranslate,
        IReadOnlyList<PartiQLExpression> arguments);
}

internal sealed class PartiQLMethodCallTranslatorProvider : IMethodCallTranslatorProvider
{
    private readonly List<IMethodCallTranslator> _defaultTranslators = new();
    private readonly List<IMethodCallTranslator> _translatorsProvidedByPlugins = new();

    public PartiQLMethodCallTranslatorProvider(IPartiQLExpressionFactory partiQLExpressionFactory, IEnumerable<IMethodCallTranslatorPlugin> translatorPlugins)
    {
        _defaultTranslators.AddRange(
            new[]
            {
                new PartiQLStringMethodsTranslator(partiQLExpressionFactory)
            });

        _translatorsProvidedByPlugins.AddRange(translatorPlugins.SelectMany(p => p.Translators));
    }

    public PartiQLExpression? Translate(
        IModel model,
        PartiQLExpression instance,
        MethodInfo methodToTranslate,
        IReadOnlyList<PartiQLExpression> arguments)
    {
        foreach (var translator in _translatorsProvidedByPlugins.Concat(_defaultTranslators))
        {
            var translationResult = translator.Translate(instance, methodToTranslate, arguments);
            if (translationResult is not null)
            {
                return translationResult;
            }
        }

        return null;
    }
}

/// <summary>
///     Represents a class that translates <see cref="string" /> based operations into their PartiQL equivalents.
/// </summary>
internal sealed class PartiQLStringMethodsTranslator : IMethodCallTranslator
{
    private static readonly MethodInfo ContainsMethodInfo = typeof(string).GetMethod(nameof(string.Contains))!;
    private static readonly MethodInfo StartsWithMethodInfo = typeof(string).GetMethod(nameof(string.StartsWith))!;

    private readonly IPartiQLExpressionFactory _partiQLExpressionFactory;

    public PartiQLStringMethodsTranslator(IPartiQLExpressionFactory partiQLExpressionFactory)
    {
        _partiQLExpressionFactory = partiQLExpressionFactory;
    }

    public PartiQLExpression? Translate(
        PartiQLExpression source,
        MethodInfo methodToTranslate,
        IReadOnlyList<PartiQLExpression> arguments)
    {
        if (StartsWithMethodInfo.Equals(methodToTranslate))
        {
            return _partiQLExpressionFactory.BeginsWith(source, arguments[0]);
        }

        if (ContainsMethodInfo.Equals(methodToTranslate))
        {
            return _partiQLExpressionFactory.Contains(source, arguments[0]);
        }

        return null;
    }
}
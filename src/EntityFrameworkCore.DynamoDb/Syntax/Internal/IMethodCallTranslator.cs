using System.Reflection;
using EntityFrameworkCore.DynamoDb.Syntax.Expressions;

namespace EntityFrameworkCore.DynamoDb.Syntax.Internal;

/// <summary>
///     Defines a contract that describes a type that translates .NET method calls into the DynamoDb equivalent. E.g.,
///     <c>string.Contains(string)</c> into <c>CONTAINS</c>.
/// </summary>
internal interface IMethodCallTranslator
{
    /// <summary>
    ///     Takes an expression and produces the DynamoDb equivalent.
    /// </summary>
    /// <param name="source">The expression.</param>
    /// <param name="methodToTranslate">The method to translate.</param>
    /// <param name="arguments">The argument.</param>
    /// <returns>The resulting <see cref="PartiQLExpression" />.</returns>
    PartiQLExpression Translate(
        PartiQLExpression source,
        MethodInfo methodToTranslate,
        IReadOnlyList<PartiQLExpression> arguments);
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

    public PartiQLExpression Translate(
        PartiQLExpression source,
        MethodInfo methodToTranslate,
        IReadOnlyList<PartiQLExpression> arguments)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (StartsWithMethodInfo.Equals(methodToTranslate))
        {
            return _partiQLExpressionFactory.BeginsWith(source, arguments[0]);
        }

        if (ContainsMethodInfo.Equals(methodToTranslate))
        {
            return _partiQLExpressionFactory.Contains(source, arguments[0]);
        }

        throw new ArgumentException(
            $"Cannot translate provided method: {methodToTranslate.Name}",
            nameof(methodToTranslate));
    }
}
using System.Reflection;
using EntityFrameworkCore.DynamoDb.Syntax.Expressions;

namespace EntityFrameworkCore.DynamoDb.Syntax.MethodTranslators;

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
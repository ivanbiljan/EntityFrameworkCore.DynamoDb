using System.Linq.Expressions;
using System.Reflection;
using EntityFrameworkCore.DynamoDb.Syntax.Expressions;

namespace EntityFrameworkCore.DynamoDb.Syntax.MethodTranslators;

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
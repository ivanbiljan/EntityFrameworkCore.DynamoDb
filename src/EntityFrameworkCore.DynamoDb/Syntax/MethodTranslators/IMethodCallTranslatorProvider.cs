using System.Linq.Expressions;
using System.Reflection;
using EntityFrameworkCore.DynamoDb.Syntax.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EntityFrameworkCore.DynamoDb.Syntax.MethodTranslators;

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
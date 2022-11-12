using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.DynamoDb.Syntax.Expressions;

/// <summary>
///     Represents a function call in a PartiQL query (i.e.,<c>EXISTS</c>, <c>ATTRIBUTE_WITH</c>, <c>BEGINS_WITH</c>,
///     <c>CONTAINS</c>, <c>MISSING</c>).
/// </summary>
public sealed class PartiQLFunctionExpression : PartiQLExpression
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="PartiQLFunctionExpression" /> class.
    /// </summary>
    /// <param name="functionName">The name of the PartiQL function being invoked.</param>
    /// <param name="arguments">The arguments.</param>
    /// <param name="type">The function's return type.</param>
    /// <param name="typeMapping">The type mapping used.</param>
    public PartiQLFunctionExpression(
        string functionName,
        IEnumerable<PartiQLExpression> arguments,
        Type type,
        CoreTypeMapping? typeMapping) : base(type, typeMapping)
    {
    }

    /// <inheritdoc />
    public override void Print(ExpressionPrinter expressionPrinter)
    {
        throw new NotImplementedException();
    }
}
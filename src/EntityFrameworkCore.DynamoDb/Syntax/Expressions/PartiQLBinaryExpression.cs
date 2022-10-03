using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.DynamoDb.Syntax.Expressions;

/// <summary>
///     Represents a binary PartiQL expression.
/// </summary>
public sealed class PartiQLBinaryExpression : PartiQLExpression
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="PartiQLBinaryExpression" /> class.
    /// </summary>
    /// <param name="left">The left side of the expression.</param>
    /// <param name="operator">The operator.</param>
    /// <param name="right">The right side of the expression.</param>
    /// <param name="typeMapping">The type mapping.</param>
    public PartiQLBinaryExpression(
        PartiQLExpression left,
        ExpressionType @operator,
        PartiQLExpression right,
        CoreTypeMapping? typeMapping) : base(left.Type, typeMapping)
    {
        Left = left;
        Operator = @operator;
        Right = right;
    }

    /// <summary>
    ///     Gets the left side of the expression.
    /// </summary>
    public PartiQLExpression Left { get; }

    /// <summary>
    ///     Gets the operator.
    /// </summary>
    public ExpressionType Operator { get; }

    /// <summary>
    ///     Gets the right side of the expression.
    /// </summary>
    public PartiQLExpression Right { get; }

    /// <inheritdoc />
    public override void Print(ExpressionPrinter expressionPrinter)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    protected override Expression VisitChildren(ExpressionVisitor visitor)
    {
        var left = (PartiQLExpression)visitor.Visit(Left);
        var right = (PartiQLExpression)visitor.Visit(Right);

        if (left != Left || right != Right)
        {
            return new PartiQLBinaryExpression(left, Operator, right, TypeMapping);
        }

        return this;
    }
}
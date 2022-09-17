using System.Linq.Expressions;
using DynamoDb.Linq.Syntax.Expressions;
using Microsoft.EntityFrameworkCore.Storage;
#pragma warning disable CS8603

namespace DynamoDb.Linq.Compilation;

/// <summary>
/// Represents a class that takes a LINQ expression tree and translates it into the equivalent PartiQL model.
/// </summary>
/// <remarks>This is the second step</remarks>
internal sealed class PartiQLTranslatingExpressionVisitor : ExpressionVisitor
{
    private readonly IPartiQLExpressionFactory _partiQLExpressionFactory;

    public PartiQLTranslatingExpressionVisitor(IPartiQLExpressionFactory partiQLExpressionFactory)
    {
        _partiQLExpressionFactory = partiQLExpressionFactory;
    }

    /// <summary>
    /// Translates the provided expression into the equivalent PartiQL expression.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <returns>The parsed expression.</returns>
    public PartiQLExpression? Translate(Expression? expression)
    {
        if (expression is null)
        {
            return null;
        }

        var result = Visit(expression);
        if (result is not PartiQLExpression partiQLExpression)
        {
            return null;
        }

        return partiQLExpression;
    }

    protected override Expression VisitUnary(UnaryExpression node)
    {
        if (Visit(node.Operand) is not PartiQLExpression operand)
        {
            return null;
        }

        switch (node.NodeType)
        {
            case ExpressionType.Not:
                return _partiQLExpressionFactory.Not(operand);
        }

        return operand;
    }
}
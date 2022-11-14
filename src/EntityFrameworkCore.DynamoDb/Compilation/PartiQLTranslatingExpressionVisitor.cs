using System.Linq.Expressions;
using EntityFrameworkCore.DynamoDb.Syntax.Expressions;
using EntityFrameworkCore.DynamoDb.Syntax.MethodTranslators;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
#pragma warning disable CS8603

namespace EntityFrameworkCore.DynamoDb.Compilation;

/// <summary>
/// Represents a class that takes a LINQ expression tree and translates it into the equivalent PartiQL model.
/// </summary>
/// <remarks>This is the second step</remarks>
internal sealed class PartiQLTranslatingExpressionVisitor : ExpressionVisitor
{
    private readonly IPartiQLExpressionFactory _partiQLExpressionFactory;
    private readonly IMethodCallTranslatorProvider _methodCallTranslatorProvider;
    private readonly QueryCompilationContext _queryCompilationContext;
    private readonly IModel _model;

    public PartiQLTranslatingExpressionVisitor(IPartiQLExpressionFactory partiQLExpressionFactory, IMethodCallTranslatorProvider methodCallTranslatorProvider, IModel model)
    {
        _partiQLExpressionFactory = partiQLExpressionFactory;
        _methodCallTranslatorProvider = methodCallTranslatorProvider;
        _model = model;
    }
    
    /// <summary>
    /// Gets detailed information about errors encountered during translation.
    /// </summary>
    public string? TranslationErrorDetails { get; private set; }

    /// <summary>
    /// Translates the provided expression into the equivalent PartiQL expression.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <returns>The parsed expression.</returns>
    public PartiQLExpression? Translate(Expression? expression)
    {
        TranslationErrorDetails = null;
        
        if (expression is null)
        {
            return null;
        }

        var result = Visit(expression);
        if (result is not PartiQLExpression partiQLExpression)
        {
            return null;
        }
        
        partiQLExpression = _partiQLExpressionFactory.apply

        return partiQLExpression;
    }

    /// <inheritdoc />
    protected override Expression VisitConstant(ConstantExpression node) => new PartiQLConstantExpression(node, null);

    /// <inheritdoc />
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
            case ExpressionType.Negate:
                return _partiQLExpressionFactory.Negate(operand);
        }

        return operand;
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        var translation = _methodCallTranslatorProvider.Translate()
        
        return base.VisitMethodCall(node);
    }
}
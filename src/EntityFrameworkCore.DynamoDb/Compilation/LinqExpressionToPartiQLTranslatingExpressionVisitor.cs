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
internal sealed class LinqExpressionToPartiQLTranslatingExpressionVisitor : ExpressionVisitor
{
    private readonly IPartiQLExpressionFactory _partiQLExpressionFactory;
    private readonly IMethodCallTranslatorProvider _methodCallTranslatorProvider;
    private readonly QueryCompilationContext _queryCompilationContext;
    private readonly IModel _model;

    public LinqExpressionToPartiQLTranslatingExpressionVisitor(IPartiQLExpressionFactory partiQLExpressionFactory, IMethodCallTranslatorProvider methodCallTranslatorProvider, IModel model, QueryCompilationContext queryCompilationContext)
    {
        _partiQLExpressionFactory = partiQLExpressionFactory;
        _methodCallTranslatorProvider = methodCallTranslatorProvider;
        _model = model;
        _queryCompilationContext = queryCompilationContext;
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
            case ExpressionType.NegateChecked:
                return _partiQLExpressionFactory.Negate(operand);
        }

        return operand;
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        if (Visit(node.Object) is not PartiQLExpression instance)
        {
            return null;
        }
        
        var arguments = new PartiQLExpression[node.Arguments.Count];
        for (var i = 0; i < arguments.Length; ++i)
        {
            if (Visit(node.Arguments[i]) is not PartiQLExpression translatedArgument)
            {
                return null;
            }

            arguments[i] = translatedArgument;
        }

        return _methodCallTranslatorProvider.Translate(_model, instance, node.Method, arguments);
    }
}
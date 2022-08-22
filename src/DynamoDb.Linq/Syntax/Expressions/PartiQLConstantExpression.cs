using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;

namespace DynamoDb.Linq.Syntax.Expressions;

public sealed class PartiQLConstantExpression : PartiQLExpression
{
    private readonly ConstantExpression _constantExpression;

    public PartiQLConstantExpression(
        Type type,
        CoreTypeMapping? coreTypeMapping,
        ConstantExpression constantExpression) : base(type, coreTypeMapping)
    {
        _constantExpression = constantExpression ?? throw new ArgumentNullException(nameof(constantExpression));
    }

    public object? Value => _constantExpression.Value;

    public override void Print(ExpressionPrinter expressionPrinter)
    {
        expressionPrinter.Append(Value?.ToString() ?? "null");
    }

    protected override Expression VisitChildren(ExpressionVisitor visitor) => this;
}
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.DynamoDb.Syntax.Expressions;

public sealed class PartiQLConstantExpression : PartiQLExpression
{
    private readonly ConstantExpression _constantExpression;

    public PartiQLConstantExpression(ConstantExpression constantExpression, CoreTypeMapping? coreTypeMapping) : base(
        constantExpression.Type,
        coreTypeMapping)
    {
        _constantExpression = constantExpression;
    }

    public object? Value => _constantExpression.Value;

    public override void Print(ExpressionPrinter expressionPrinter)
    {
        expressionPrinter.Append(Value?.ToString() ?? "null");
    }

    protected override Expression VisitChildren(ExpressionVisitor visitor) => this;
}
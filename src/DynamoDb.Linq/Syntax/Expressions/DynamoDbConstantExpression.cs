using System.Linq.Expressions;

namespace DynamoDb.Linq.Syntax.Expressions;

public sealed class DynamoDbConstantExpression : DynamoDbExpression
{
    private readonly ConstantExpression _constantExpression;

    public DynamoDbConstantExpression(ConstantExpression constantExpression)
    {
        _constantExpression = constantExpression ?? throw new ArgumentNullException(nameof(constantExpression));
    }

    public object? Value => _constantExpression.Value;

    protected override Expression VisitChildren(ExpressionVisitor visitor) => this;
}
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;

namespace DynamoDb.Linq.Syntax.Expressions;

public abstract class DynamoDbExpression : Expression, IPrintableExpression
{
    public DynamoDbExpression(Type type, CoreTypeMapping? typeMapping)
    {
        Type = type;
        TypeMapping = typeMapping;
    }

    public sealed override ExpressionType NodeType
        => ExpressionType.Extension;

    public override Type Type { get; }
    
    public CoreTypeMapping? TypeMapping { get; }

    public abstract void Print(ExpressionPrinter expressionPrinter);
    
    protected override Expression VisitChildren(ExpressionVisitor visitor) => throw new NotImplementedException();
}
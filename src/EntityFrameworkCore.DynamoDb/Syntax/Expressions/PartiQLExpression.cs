using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.DynamoDb.Syntax.Expressions;

public abstract class PartiQLExpression : Expression, IPrintableExpression
{
    protected PartiQLExpression(Type type, CoreTypeMapping? typeMapping)
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
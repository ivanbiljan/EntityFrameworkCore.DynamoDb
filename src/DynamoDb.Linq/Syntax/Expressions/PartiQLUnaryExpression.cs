using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;

namespace DynamoDb.Linq.Syntax.Expressions;

public sealed class PartiQLUnaryExpression : PartiQLExpression
{
    public PartiQLUnaryExpression(ExpressionType nodeType, PartiQLExpression operand, CoreTypeMapping? typeMapping) :
        base(operand.Type, typeMapping)
    {
        NodeType = nodeType;
        Operand = operand;
    }

    public ExpressionType NodeType { get; }
    
    public PartiQLExpression Operand { get; }

    public override void Print(ExpressionPrinter expressionPrinter)
    {
        throw new NotImplementedException();
    }

    protected override Expression VisitChildren(ExpressionVisitor visitor)
    {
        var operand = (PartiQLExpression)visitor.Visit(Operand);

        return operand != Operand ? new PartiQLUnaryExpression(operand.NodeType, operand, operand.TypeMapping) : this;
    }
}
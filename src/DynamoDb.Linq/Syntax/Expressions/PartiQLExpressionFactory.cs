using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace DynamoDb.Linq.Syntax.Expressions;

/// <summary>
/// Represents a class that creates <see cref="PartiQLExpression"/>s.
/// </summary>
internal static class PartiQLExpressionFactory
{
    /// <summary>
    /// Produces a constant expression.
    /// </summary>
    /// <param name="value">The constant.</param>
    /// <param name="coreTypeMapping">The <see cref="CoreTypeMapping"/>.</param>
    /// <returns>The created <see cref="PartiQLConstantExpression"/>.</returns>
    public static PartiQLConstantExpression MakeConstant(object value, CoreTypeMapping? coreTypeMapping) =>
        new(Expression.Constant(value), coreTypeMapping);

    public static PartiQLBinaryExpression AndAlso(PartiQLExpression left, PartiQLExpression right) =>
        new(left, ExpressionType.AndAlso, right);

    public static PartiQLBinaryExpression OrElse(PartiQLExpression left, PartiQLExpression right) =>
        new(left, ExpressionType.OrElse, right);

    public static PartiQLBinaryExpression Equal(PartiQLExpression left, PartiQLExpression right) =>
        new(left, ExpressionType.Equal, right);
    
    public static PartiQLBinaryExpression NotEqual(PartiQLExpression left, PartiQLExpression right) =>
        new(left, ExpressionType.NotEqual, right);
    
    public static PartiQLBinaryExpression LessThan(PartiQLExpression left, PartiQLExpression right) =>
        new(left, ExpressionType.LessThan, right);
    
    public static PartiQLBinaryExpression LessThanOrEqual(PartiQLExpression left, PartiQLExpression right) =>
        new(left, ExpressionType.LessThanOrEqual, right);
    
    public static PartiQLBinaryExpression GreaterThan(PartiQLExpression left, PartiQLExpression right) =>
        new(left, ExpressionType.GreaterThan, right);
    
    public static PartiQLBinaryExpression GreaterThanOrEqual(PartiQLExpression left, PartiQLExpression right) =>
        new(left, ExpressionType.GreaterThanOrEqual, right);
}
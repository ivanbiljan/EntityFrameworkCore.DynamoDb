﻿using System.Linq.Expressions;
using DynamoDb.Linq.Compilation;
using Microsoft.EntityFrameworkCore.Storage;

namespace DynamoDb.Linq.Syntax.Expressions;

/// <summary>
/// Specifies a contract for a factory that produces PartiQL expressions.
/// </summary>
internal interface IPartiQLExpressionFactory
{
    /// <summary>
    /// Produces a PartiQL AndAlso (&&) expression.
    /// </summary>
    /// <param name="left">The first operand.</param>
    /// <param name="right">The second operand.</param>
    /// <returns>The <see cref="PartiQLBinaryExpression"/>.</returns>
    PartiQLBinaryExpression AndAlso(PartiQLExpression left, PartiQLExpression right);
    
    /// <summary>
    /// Produces a PartiQL Equal (==) expression.
    /// </summary>
    /// <param name="left">The first operand.</param>
    /// <param name="right">The second operand.</param>
    /// <returns>The <see cref="PartiQLBinaryExpression"/>.</returns>
    PartiQLBinaryExpression Equal(PartiQLExpression left, PartiQLExpression right);
    
    /// <summary>
    /// Produces a PartiQL GreaterThan (>) expression.
    /// </summary>
    /// <param name="left">The first operand.</param>
    /// <param name="right">The second operand.</param>
    /// <returns>The <see cref="PartiQLBinaryExpression"/>.</returns>
    PartiQLBinaryExpression GreaterThan(PartiQLExpression left, PartiQLExpression right);
    
    /// <summary>
    /// Produces a PartiQL GreaterThanOrEqual (>=) expression.
    /// </summary>
    /// <param name="left">The first operand.</param>
    /// <param name="right">The second operand.</param>
    /// <returns>The <see cref="PartiQLBinaryExpression"/>.</returns>
    PartiQLBinaryExpression GreaterThanOrEqual(PartiQLExpression left, PartiQLExpression right);
    
    /// <summary>
    /// Produces a PartiQL LessThan (&lt;) expression.
    /// </summary>
    /// <param name="left">The first operand.</param>
    /// <param name="right">The second operand.</param>
    /// <returns>The <see cref="PartiQLBinaryExpression"/>.</returns>
    PartiQLBinaryExpression LessThan(PartiQLExpression left, PartiQLExpression right);
    
    /// <summary>
    /// Produces a PartiQL LessThanOrEqual (&lt;=) expression.
    /// </summary>
    /// <param name="left">The first operand.</param>
    /// <param name="right">The second operand.</param>
    /// <returns>The <see cref="PartiQLBinaryExpression"/>.</returns>
    PartiQLBinaryExpression LessThanOrEqual(PartiQLExpression left, PartiQLExpression right);

    /// <summary>
    ///     Produces a PartiQL constant expression.
    /// </summary>
    /// <param name="value">The constant.</param>
    /// <param name="coreTypeMapping">The <see cref="CoreTypeMapping" />.</param>
    /// <returns>The <see cref="PartiQLConstantExpression" />.</returns>
    PartiQLConstantExpression MakeConstant(object value, CoreTypeMapping? coreTypeMapping);
    
    /// <summary>
    /// Produces a PartiQL NotEqual (!=) expression. In DynamoDb terms this is the NOT operator.
    /// </summary>
    /// <param name="left">The first operand.</param>
    /// <param name="right">The second operand.</param>
    /// <returns>The <see cref="PartiQLBinaryExpression"/>.</returns>
    PartiQLBinaryExpression NotEqual(PartiQLExpression left, PartiQLExpression right);
    
    /// <summary>
    /// Produces a PartiQL OrElse (||) expression. In DynamoDb terms this is the OR operator.
    /// </summary>
    /// <param name="left">The first operand.</param>
    /// <param name="right">The second operand.</param>
    /// <returns>The <see cref="PartiQLBinaryExpression"/>.</returns>
    PartiQLBinaryExpression OrElse(PartiQLExpression left, PartiQLExpression right);
}


/// <summary>
///     Represents a class that creates <see cref="PartiQLExpression" />s.
/// </summary>
internal class PartiQLExpressionFactory : IPartiQLExpressionFactory
{
    /// <inheritdoc />
    public PartiQLBinaryExpression AndAlso(PartiQLExpression left, PartiQLExpression right) =>
        new(left, ExpressionType.AndAlso, right);

    /// <inheritdoc />
    public PartiQLBinaryExpression Equal(PartiQLExpression left, PartiQLExpression right) =>
        new(left, ExpressionType.Equal, right);

    /// <inheritdoc />
    public PartiQLBinaryExpression GreaterThan(PartiQLExpression left, PartiQLExpression right) =>
        new(left, ExpressionType.GreaterThan, right);

    /// <inheritdoc />
    public PartiQLBinaryExpression GreaterThanOrEqual(PartiQLExpression left, PartiQLExpression right) =>
        new(left, ExpressionType.GreaterThanOrEqual, right);

    /// <inheritdoc />
    public PartiQLBinaryExpression LessThan(PartiQLExpression left, PartiQLExpression right) =>
        new(left, ExpressionType.LessThan, right);

    /// <inheritdoc />
    public PartiQLBinaryExpression LessThanOrEqual(PartiQLExpression left, PartiQLExpression right) =>
        new(left, ExpressionType.LessThanOrEqual, right);
    
    /// <inheritdoc />
    public PartiQLConstantExpression MakeConstant(object value, CoreTypeMapping? coreTypeMapping) =>
        new(Expression.Constant(value), coreTypeMapping);

    /// <inheritdoc />
    public PartiQLBinaryExpression NotEqual(PartiQLExpression left, PartiQLExpression right) =>
        new(left, ExpressionType.NotEqual, right);

    /// <inheritdoc />
    public PartiQLBinaryExpression OrElse(PartiQLExpression left, PartiQLExpression right) =>
        new(left, ExpressionType.OrElse, right);
}
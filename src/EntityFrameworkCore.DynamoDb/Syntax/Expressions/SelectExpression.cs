using System.Linq.Expressions;
using EntityFrameworkCore.DynamoDb.Syntax.Expressions;

/// <summary>
///     Represents a projection in a SELECT statement.
/// </summary>
internal sealed class ProjectionExpression : Expression
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ProjectionExpression" />.
    /// </summary>
    /// <param name="expression">The value being projected.</param>
    public ProjectionExpression(PartiQLExpression expression)
    {
        Expression = expression;
    }

    /// <summary>
    ///     Gets the value being projected.
    /// </summary>
    public PartiQLExpression Expression { get; }
}

/// <summary>
///     Represents an <c>ORDER BY</c> expression.
/// </summary>
internal sealed class OrderByExpression : Expression
{
    public OrderByExpression(PartiQLExpression expression, bool isDescending)
    {
        Expression = expression;
        IsDescending = isDescending;
    }

    public PartiQLExpression Expression { get; }
    public bool IsDescending { get; }
}

/// <summary>
/// Represents a SELECT expression.
/// </summary>
internal sealed class SelectExpression : Expression
{
    private readonly List<ProjectionExpression> _projections = new();
    private readonly List<OrderByExpression> _orderByExpressions = new();
}
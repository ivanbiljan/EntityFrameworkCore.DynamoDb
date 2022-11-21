using System.Linq.Expressions;
using EntityFrameworkCore.DynamoDb.Extensions;
using EntityFrameworkCore.DynamoDb.Syntax.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;

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

internal sealed class FromExpression : Expression
{
    public IEntityType ReferencedEntity { get; }

    public FromExpression(IEntityType referencedEntity)
    {
        ReferencedEntity = referencedEntity;
    }

    /// <inheritdoc />
    public override Type Type => ReferencedEntity.ClrType;

    /// <inheritdoc />
    public override ExpressionType NodeType => ExpressionType.Extension;

    /// <inheritdoc />
    protected override Expression VisitChildren(ExpressionVisitor visitor) => this;

    /// <inheritdoc />
    public override string ToString() => $"\"{ReferencedEntity.DisplayName()}\"";
}

internal sealed class AttributeAccessExpression : PartiQLExpression
{
    public IProperty Property { get; }
    public FromExpression FromExpression { get; }

    public AttributeAccessExpression(IProperty property, FromExpression fromExpression) : base(
        property.ClrType,
        property.FindTypeMapping())
    {
        Property = property;
        FromExpression = fromExpression;
    }

    public override void Print(ExpressionPrinter expressionPrinter)
    {
        expressionPrinter.Append($"{FromExpression}.{Property.GetDynamoDbAttributeName()}");
    }
}

internal sealed class EntityProjectionExpression : Expression
{
    private readonly IDictionary<IProperty, AttributeAccessExpression> _propertyExpressions =
        new Dictionary<IProperty, AttributeAccessExpression>();
    private readonly IEntityType _entityType;

    public EntityProjectionExpression(IEntityType entityType, FromExpression accessExpression)
    {
        _entityType = entityType;
        AccessExpression = accessExpression;
    }

    public override ExpressionType NodeType => ExpressionType.Extension;
    public override Type Type => _entityType.ClrType;

    public FromExpression AccessExpression { get; }

    protected override Expression VisitChildren(ExpressionVisitor visitor)
    {
        var accessExpression = (FromExpression)visitor.Visit(AccessExpression);

        return accessExpression != AccessExpression
            ? new EntityProjectionExpression(_entityType, accessExpression)
            : this;
    }

    public AttributeAccessExpression GetProperty(IProperty property)
    {
        if (_propertyExpressions.TryGetValue(property, out var expression))
        {
            return expression;
        }

        expression = new AttributeAccessExpression(property, AccessExpression);
        _propertyExpressions[property] = expression;

        return expression;
    }
}

/// <summary>
/// Represents a SELECT expression.
/// </summary>
internal sealed class SelectExpression : Expression
{
    private readonly Dictionary<ProjectionMember, Expression> _projectionMemberToExpressionMap = new();
    private readonly List<ProjectionExpression> _projections = new();
    private readonly List<OrderByExpression> _orderByExpressions = new();

    public SelectExpression(IEntityType entityType)
    {
        FromExpression = new FromExpression(entityType);
    }
    
    public FromExpression FromExpression { get; }

    public IReadOnlyList<ProjectionExpression> Projections => _projections;

    public IReadOnlyList<OrderByExpression> Orderings => _orderByExpressions;
    
    public PartiQLExpression? Filter { get; private set; }

    public void ApplyFilter(PartiQLExpression filter)
    {
        Filter = Filter is null
            ? filter
            : new PartiQLBinaryExpression(Filter, ExpressionType.AndAlso, filter, filter.TypeMapping);
    }

    public void ReplaceProjectionMapping(IDictionary<ProjectionMember, Expression> projectionMap)
    {
        _projectionMemberToExpressionMap.Clear();
        foreach (var (key, value) in projectionMap)
        {
            _projectionMemberToExpressionMap[key] = value;
        }
    }

    public Expression? GetProjection(ProjectionMember projectionMember) =>
        _projectionMemberToExpressionMap.TryGetValue(projectionMember, out var result) ? result : null;
}
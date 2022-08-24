using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DynamoDb.Linq.Syntax.Expressions;

/// <summary>
/// Represents a SELECT expression. See <see cref="!:https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ql-reference.select.html">Amazon documentation</see> for more information.
/// </summary>
public sealed class SelectExpression : Expression
{
    /// <summary>
    /// Gets a collection of expressions that represent a list of attributes in the final result set.
    /// </summary>
    public IEnumerable<PartiQLProjectionExpression> Projections { get; } = new List<PartiQLProjectionExpression>();
    
    /// <summary>
    /// Gets an expression that represents the table (essentially the <see cref="IEntityType"/>) the data is requested from.
    /// </summary>
    public PartiQLFromExpression From { get; }
    
    /// <summary>
    /// Gets an expression that represents the criteria used to filter the data.
    /// </summary>
    public PartiQLExpression Where { get; }

    /// <summary>
    /// Gets a collection of expressions that determine how the result set is ordered.
    /// </summary>
    public IEnumerable<PartiQLOrderByExpression> OrderBys { get; } = new List<PartiQLOrderByExpression>();
}

public class PartiQLOrderByExpression
{
}

public class PartiQLFromExpression
{
}

public class PartiQLProjectionExpression
{
}
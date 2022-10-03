namespace EntityFrameworkCore.DynamoDb.Syntax;

/// <summary>
///     Represents a PartiQL query.
/// </summary>
public sealed class PartiQLQuery
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="PartiQLQuery" /> class.
    /// </summary>
    /// <param name="query">The query to be executed.</param>
    /// <param name="parameters">A list of query parameters.</param>
    public PartiQLQuery(string query, IList<PartiQLParameter> parameters)
    {
        Query = query;
        Parameters = parameters;
    }

    /// <summary>
    ///     Gets a list of <see cref="PartiQLParameter" /> that will be substituted when the <see cref="Query" /> is executed.
    /// </summary>
    public IList<PartiQLParameter> Parameters { get; }

    /// <summary>
    ///     Gets the query string DynamoDb will execute.
    /// </summary>
    public string Query { get; }
}
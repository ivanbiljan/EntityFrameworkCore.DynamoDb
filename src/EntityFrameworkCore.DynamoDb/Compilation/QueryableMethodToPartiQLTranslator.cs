using System.Linq.Expressions;
using EntityFrameworkCore.DynamoDb.Syntax.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.DynamoDb.Compilation;

/// <inheritdoc />
internal sealed class PartiQLQueryableMethodTranslatingExpressionVisitorFactory : IQueryableMethodTranslatingExpressionVisitorFactory
{
    private readonly QueryableMethodTranslatingExpressionVisitorDependencies _dependencies;
    private readonly QueryCompilationContext _queryCompilationContext;
    private readonly LinqExpressionToPartiQLTranslatingExpressionVisitor _partiQLTranslator;

    public PartiQLQueryableMethodTranslatingExpressionVisitorFactory(QueryableMethodTranslatingExpressionVisitorDependencies dependencies, QueryCompilationContext queryCompilationContext, LinqExpressionToPartiQLTranslatingExpressionVisitor partiQLTranslator)
    {
        _dependencies = dependencies;
        _queryCompilationContext = queryCompilationContext;
        _partiQLTranslator = partiQLTranslator;
    }

    /// <inheritdoc />
    public QueryableMethodTranslatingExpressionVisitor Create(QueryCompilationContext queryCompilationContext) =>
        new PartiQLQueryableMethodTranslatingExpressionVisitor(
            _dependencies,
            _queryCompilationContext,
            false,
            _partiQLTranslator);
}

/// <inheritdoc />
internal sealed class PartiQLQueryableMethodTranslatingExpressionVisitor : QueryableMethodTranslatingExpressionVisitor
{
    private readonly LinqExpressionToPartiQLTranslatingExpressionVisitor _partiQLTranslator;
    private readonly PartiQLExpressionFactory _partiQLExpressionFactory;
    private readonly IModel _model;

    public PartiQLQueryableMethodTranslatingExpressionVisitor(QueryableMethodTranslatingExpressionVisitorDependencies dependencies, QueryCompilationContext queryCompilationContext, bool subquery, LinqExpressionToPartiQLTranslatingExpressionVisitor partiQLTranslator) : base(dependencies, queryCompilationContext, subquery)
    {
        _partiQLTranslator = partiQLTranslator;
    }

    /// <inheritdoc />
    protected override QueryableMethodTranslatingExpressionVisitor CreateSubqueryVisitor() => throw new NotImplementedException();

    /// <inheritdoc />
    protected override ShapedQueryExpression CreateShapedQueryExpression(IEntityType entityType)
    {
        var queryExpression = _partiQLExpressionFactory.Select(entityType);
        var shaperExpression = new EntityShaperExpression(
            entityType,
            new ProjectionBindingExpression(queryExpression, new ProjectionMember(), typeof(ValueBuffer)),
            false);

        return new ShapedQueryExpression(queryExpression, shaperExpression);
    }

    /// <inheritdoc />
    protected override ShapedQueryExpression? TranslateAll(ShapedQueryExpression source, LambdaExpression predicate) => throw new NotImplementedException();

    /// <inheritdoc />
    protected override ShapedQueryExpression? TranslateAny(ShapedQueryExpression source, LambdaExpression? predicate) => throw new NotImplementedException();

    /// <inheritdoc />
    protected override ShapedQueryExpression? TranslateAverage(ShapedQueryExpression source, LambdaExpression? selector, Type resultType) => throw new NotImplementedException();

    /// <inheritdoc />
    protected override ShapedQueryExpression? TranslateCast(ShapedQueryExpression source, Type castType) => throw new NotImplementedException();

    /// <inheritdoc />
    protected override ShapedQueryExpression? TranslateConcat(ShapedQueryExpression source1, ShapedQueryExpression source2) => throw new NotImplementedException();

    /// <inheritdoc />
    protected override ShapedQueryExpression? TranslateContains(ShapedQueryExpression source, Expression item) => throw new NotImplementedException();

    /// <inheritdoc />
    protected override ShapedQueryExpression? TranslateCount(ShapedQueryExpression source, LambdaExpression? predicate) => throw new NotImplementedException();

    /// <inheritdoc />
    protected override ShapedQueryExpression? TranslateDefaultIfEmpty(ShapedQueryExpression source, Expression? defaultValue) => throw new NotImplementedException();

    /// <inheritdoc />
    protected override ShapedQueryExpression? TranslateDistinct(ShapedQueryExpression source) => throw new NotImplementedException();

    /// <inheritdoc />
    protected override ShapedQueryExpression? TranslateElementAtOrDefault(ShapedQueryExpression source, Expression index, bool returnDefault) => throw new NotImplementedException();

    /// <inheritdoc />
    protected override ShapedQueryExpression? TranslateExcept(ShapedQueryExpression source1, ShapedQueryExpression source2) => throw new NotImplementedException();

    /// <inheritdoc />
    protected override ShapedQueryExpression? TranslateFirstOrDefault(
        ShapedQueryExpression source,
        LambdaExpression? predicate,
        Type returnType,
        bool returnDefault) =>
        throw new NotImplementedException();

    /// <inheritdoc />
    protected override ShapedQueryExpression? TranslateGroupBy(
        ShapedQueryExpression source,
        LambdaExpression keySelector,
        LambdaExpression? elementSelector,
        LambdaExpression? resultSelector) =>
        throw new NotImplementedException();

    /// <inheritdoc />
    protected override ShapedQueryExpression? TranslateGroupJoin(
        ShapedQueryExpression outer,
        ShapedQueryExpression inner,
        LambdaExpression outerKeySelector,
        LambdaExpression innerKeySelector,
        LambdaExpression resultSelector) =>
        throw new NotImplementedException();
    
    /// <inheritdoc />
    protected override ShapedQueryExpression? TranslateIntersect(ShapedQueryExpression source1, ShapedQueryExpression source2) => throw new NotImplementedException();
    
    /// <inheritdoc />
    protected override ShapedQueryExpression? TranslateJoin(
        ShapedQueryExpression outer,
        ShapedQueryExpression inner,
        LambdaExpression outerKeySelector,
        LambdaExpression innerKeySelector,
        LambdaExpression resultSelector) =>
        throw new NotImplementedException();

    /// <inheritdoc />
    protected override ShapedQueryExpression? TranslateLeftJoin(
        ShapedQueryExpression outer,
        ShapedQueryExpression inner,
        LambdaExpression outerKeySelector,
        LambdaExpression innerKeySelector,
        LambdaExpression resultSelector) =>
        throw new NotImplementedException();

    /// <inheritdoc />
    protected override ShapedQueryExpression? TranslateLastOrDefault(
        ShapedQueryExpression source,
        LambdaExpression? predicate,
        Type returnType,
        bool returnDefault) =>
        throw new NotImplementedException();
    
    /// <inheritdoc />
    protected override ShapedQueryExpression? TranslateLongCount(ShapedQueryExpression source, LambdaExpression? predicate) => throw new NotImplementedException();

    /// <inheritdoc />
    protected override ShapedQueryExpression? TranslateMax(ShapedQueryExpression source, LambdaExpression? selector, Type resultType) => throw new NotImplementedException();

    /// <inheritdoc />
    protected override ShapedQueryExpression? TranslateMin(ShapedQueryExpression source, LambdaExpression? selector, Type resultType) => throw new NotImplementedException();

    /// <inheritdoc />
    protected override ShapedQueryExpression? TranslateOfType(ShapedQueryExpression source, Type resultType) => throw new NotImplementedException();

    /// <inheritdoc />
    protected override ShapedQueryExpression? TranslateOrderBy(ShapedQueryExpression source, LambdaExpression keySelector, bool ascending) => throw new NotImplementedException();

    /// <inheritdoc />
    protected override ShapedQueryExpression? TranslateReverse(ShapedQueryExpression source) => throw new NotImplementedException();

    /// <inheritdoc />
    protected override ShapedQueryExpression TranslateSelect(ShapedQueryExpression source, LambdaExpression selector) => throw new NotImplementedException();

    /// <inheritdoc />
    protected override ShapedQueryExpression? TranslateSelectMany(
        ShapedQueryExpression source,
        LambdaExpression collectionSelector,
        LambdaExpression resultSelector) =>
        throw new NotImplementedException();

    /// <inheritdoc />
    protected override ShapedQueryExpression? TranslateSelectMany(ShapedQueryExpression source, LambdaExpression selector) => throw new NotImplementedException();

    /// <inheritdoc />
    protected override ShapedQueryExpression? TranslateSingleOrDefault(
        ShapedQueryExpression source,
        LambdaExpression? predicate,
        Type returnType,
        bool returnDefault) =>
        throw new NotImplementedException();

    /// <inheritdoc />
    protected override ShapedQueryExpression? TranslateSkip(ShapedQueryExpression source, Expression count) => throw new NotImplementedException();

    /// <inheritdoc />
    protected override ShapedQueryExpression? TranslateSkipWhile(ShapedQueryExpression source, LambdaExpression predicate) => throw new NotImplementedException();

    /// <inheritdoc />
    protected override ShapedQueryExpression? TranslateSum(ShapedQueryExpression source, LambdaExpression? selector, Type resultType) => throw new NotImplementedException();

    /// <inheritdoc />
    protected override ShapedQueryExpression? TranslateTake(ShapedQueryExpression source, Expression count) => throw new NotImplementedException();

    /// <inheritdoc />
    protected override ShapedQueryExpression? TranslateTakeWhile(ShapedQueryExpression source, LambdaExpression predicate) => throw new NotImplementedException();

    /// <inheritdoc />
    protected override ShapedQueryExpression? TranslateThenBy(ShapedQueryExpression source, LambdaExpression keySelector, bool ascending) => throw new NotImplementedException();

    /// <inheritdoc />
    protected override ShapedQueryExpression? TranslateUnion(ShapedQueryExpression source1, ShapedQueryExpression source2) => throw new NotImplementedException();

    /// <inheritdoc />
    protected override ShapedQueryExpression? TranslateWhere(ShapedQueryExpression source, LambdaExpression predicate)
    {
        var filter = TranslateLambdaExpression(predicate, source);
        if (filter is null)
        {
            throw new InvalidOperationException();
        }
        
        ((SelectExpression)source.QueryExpression).ApplyFilter(filter);

        return source;
    }

    private PartiQLExpression? TranslateLambdaExpression(LambdaExpression lambdaExpression, ShapedQueryExpression shapedQueryExpression)
    {
        var body = ReplacingExpressionVisitor.Replace(
            lambdaExpression.Parameters.Single(),
            shapedQueryExpression.ShaperExpression,
            lambdaExpression.Body);

        return _partiQLTranslator.Translate(body);
    }
}
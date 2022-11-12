using System.Reflection;
using EntityFrameworkCore.DynamoDb.Syntax.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EntityFrameworkCore.DynamoDb.Syntax.MethodTranslators;

internal sealed class PartiQLMethodCallTranslatorProvider : IMethodCallTranslatorProvider
{
    private readonly List<IMethodCallTranslator> _defaultTranslators = new();
    private readonly List<IMethodCallTranslator> _translatorsProvidedByPlugins = new();

    public PartiQLMethodCallTranslatorProvider(IPartiQLExpressionFactory partiQLExpressionFactory, IEnumerable<IMethodCallTranslatorPlugin> translatorPlugins)
    {
        _defaultTranslators.AddRange(
            new[]
            {
                new PartiQLStringMethodsTranslator(partiQLExpressionFactory)
            });

        _translatorsProvidedByPlugins.AddRange(translatorPlugins.SelectMany(p => p.Translators));
    }

    public PartiQLExpression? Translate(
        IModel model,
        PartiQLExpression instance,
        MethodInfo methodToTranslate,
        IReadOnlyList<PartiQLExpression> arguments)
    {
        foreach (var translator in _translatorsProvidedByPlugins.Concat(_defaultTranslators))
        {
            var translationResult = translator.Translate(instance, methodToTranslate, arguments);
            if (translationResult is not null)
            {
                return translationResult;
            }
        }

        return null;
    }
}
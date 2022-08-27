using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace DynamoDb.Linq;

public static class DependencyInjection
{
    /// <summary>
    /// Configures services required for EntityFrameworkCore.DynamoDb.
    /// </summary>
    /// <param name="serviceCollection">The <see cref="IServiceCollection"/> the services will be added to.</param>
    /// <returns>The modified <paramref name="serviceCollection"/>.</returns>
    public static IServiceCollection AddEntityFrameworkDynamoDb(this IServiceCollection serviceCollection)
    {
        var builder = new EntityFrameworkServicesBuilder(serviceCollection);

        builder.TryAddCoreServices();

        return serviceCollection;
    }
}
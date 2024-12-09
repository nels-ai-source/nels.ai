using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Postgres;
using Microsoft.SemanticKernel.Memory;

namespace Nels.SemanticKernel.Connectors.Memory.Postgres;

/// <summary>
/// Register a Postgres <see cref="IMemoryStore"/> with the specified service ID and where <see cref="PostgresClient"/> is retrieved from the dependency injection container.
/// </summary>
/// <param name="services">The <see cref="IServiceCollection"/> to register the <see cref="IMemoryStore"/> on.</param>
/// <param name="options">Optional options to further configure the <see cref="IMemoryStore"/>.</param>
/// <param name="serviceId">An optional service id to use as the service key.</param>
/// <returns>The service collection.</returns>
public static class PostgresServiceCollectionExtensions
{
    internal const string DefaultSchema = "public";
    /// <summary>
    /// Register a Postgres <see cref="IMemoryStore"/> with the specified service ID and where <see cref="PostgresClient"/> is constructed using the provided parameters.
    /// </summary>
    /// <param name="builder">The <see cref="IKernelBuilder"/> to register the <see cref="IMemoryStore"/> on.</param>
    /// <param name="connectionString">Postgres database connection string.</param>
    /// <param name="vectorSize">Embedding vector size.</param>
    /// <param name="schema">Schema of collection tables.</param>
    /// <param name="serviceId">An optional service id to use as the service key.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddPostgresMemoryStore(this IKernelBuilder builder, string connectionString, int vectorSize, string? schema = DefaultSchema, string? serviceId = default)
    {
        return builder.Services.AddPostgresMemoryStore(connectionString, vectorSize, schema, serviceId);
    }
    /// <summary>
    /// Register a Postgres <see cref="IMemoryStore"/> with the specified service ID and where <see cref="PostgresClient"/> is constructed using the provided parameters.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to register the <see cref="IMemoryStore"/> on.</param>
    /// <param name="connectionString">Postgres database connection string.</param>
    /// <param name="vectorSize">Embedding vector size.</param>
    /// <param name="schema">Schema of collection tables.</param>
    /// <param name="serviceId">An optional service id to use as the service key.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddPostgresMemoryStore(this IServiceCollection services, string connectionString, int vectorSize, string? schema = DefaultSchema, string? serviceId = default)
    {
        services.AddKeyedSingleton<IMemoryStore>(
            serviceId,
            (sp, obj) =>
            {
                return new PostgresMemoryStore(connectionString, vectorSize, schema);
            });

        return services;
    }

}

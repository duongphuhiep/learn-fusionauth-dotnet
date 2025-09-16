using Microsoft.Extensions.DependencyInjection;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace FusionAuth.Sdk;

/// <summary>
/// Register a Kiota API client in .NET with dependency injection.
/// https://learn.microsoft.com/en-us/openapi/kiota/tutorials/dotnet-dependency-injection
/// </summary>
public static class KiotaServiceCollectionExtensions
{
    /// <summary>
    /// Register Kiota default handlers
    /// </summary>
    public static IServiceCollection AddKiotaHandlers(this IServiceCollection services)
    {
        // register default Kiota handler types (if KiotaClientFactory exposes them)
        var handlerTypes = KiotaClientFactory.GetDefaultHandlerActivatableTypes();
        foreach (var hType in handlerTypes)
            services.AddTransient(hType);
        return services;
    }

    /// <summary>
    /// Attach the registered handlers to the typed HttpClient pipeline
    /// </summary>
    public static IHttpClientBuilder AttachKiotaHandlers(this IHttpClientBuilder builder)
    {
        var handlerTypes = KiotaClientFactory.GetDefaultHandlerActivatableTypes();
        foreach (var hType in handlerTypes)
        {
            // each handler type is resolved from DI and added to the pipeline
            builder.AddHttpMessageHandler(sp => (DelegatingHandler)sp.GetRequiredService(hType));
        }
        
        return builder;
    }
}

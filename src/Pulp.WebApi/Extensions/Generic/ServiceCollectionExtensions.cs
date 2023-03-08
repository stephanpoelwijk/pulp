using Pulp.WebApi.Import.OpenApi;

namespace Pulp.WebApi.Extensions.Generic;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGenericApiService(this IServiceCollection services, IConfiguration configuration)
    {
        var extensionConfiguration = new EndpointConfiguration(configuration);

        services.AddSingleton<IValueGenerator, ValueGenerator>();
        services.AddSingleton(extensionConfiguration);
        services.AddSingleton<OpenApiImporter>();

        return services;
    }
}

namespace Pulp.WebApi.Extensions.OpenApi;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOpenApiExtensionServices(this IServiceCollection services, IConfiguration configuration)
    {
        var extensionConfiguration = new OpenApiConfiguration(configuration);

        services.AddSingleton(extensionConfiguration);
        services.AddSingleton<IObjectGenerator, ObjectGenerator>();

        return services;
    }
}

namespace Pulp.WebApi.Extensions.FileSystem
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFileSystemEndpointServices(this IServiceCollection services, IConfiguration configuration)
        {
            var extensionConfiguration = new FileSystemConfiguration(configuration);

            services.AddSingleton(extensionConfiguration);

            return services;
        }
    }
}

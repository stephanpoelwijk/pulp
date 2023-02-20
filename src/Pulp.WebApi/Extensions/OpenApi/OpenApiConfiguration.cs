namespace Pulp.WebApi.Extensions.OpenApi;

public sealed class OpenApiConfiguration
{
    public string SpecFileName { get; init; }

    internal OpenApiConfiguration(IConfiguration configuration)
    {
        var specFileName = configuration.GetValue<string>("Extensions:OpenApi:Spec");
        if (specFileName is null)
        {
            throw new Exception($"Configuration Extensions:OpenApi:Spec could not be found or is empty");
        }

        SpecFileName = specFileName;
    }
}

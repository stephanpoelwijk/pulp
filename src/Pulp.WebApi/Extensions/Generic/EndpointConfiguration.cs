namespace Pulp.WebApi.Extensions.Generic;

public class EndpointConfiguration
{
    public string OpenApiSpecFileName { get; }

    internal EndpointConfiguration(IConfiguration configuration)
    {
        var specFileName = configuration.GetValue<string>("Extensions:OpenApi:Spec");
        if (specFileName is null)
        {
            throw new Exception($"Configuration Extensions:OpenApi:Spec could not be found or is empty");
        }

        OpenApiSpecFileName = specFileName;
    }
}

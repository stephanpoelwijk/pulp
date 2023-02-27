using Microsoft.OpenApi.Models;

namespace Pulp.WebApi.Extensions.OpenApi;

public interface IObjectGenerator
{
    object GenerateObjectFromSchema(OpenApiSchema schema);
}

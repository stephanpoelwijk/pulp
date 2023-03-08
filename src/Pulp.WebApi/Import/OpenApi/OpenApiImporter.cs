using Microsoft.OpenApi.Readers;
using Pulp.WebApi.Extensions.Generic.Models;
using Pulp.WebApi.Import.OpenApi.Converters;

namespace Pulp.WebApi.Import.OpenApi;

public class OpenApiImporter
{
    public BackendService Import(string openApiFileName)
    {
        using var stream = new FileStream(openApiFileName, FileMode.Open, FileAccess.Read);
        var document = new OpenApiStreamReader().Read(stream, out var diagnostic);

        var backendService = new BackendService(Path.GetFileNameWithoutExtension(openApiFileName),
            document.Paths.SelectMany(p => OpenApiPathConverter.ToEndpoints(p.Key, p.Value)));

        return backendService;
    }
}

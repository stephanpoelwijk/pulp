using CsvHelper;
using Microsoft.AspNetCore.StaticFiles;
using System.Globalization;

namespace Pulp.WebApi.Extensions.FileSystem;

public static class FileSystemExtension
{
    private const string ConfigurationFileName = "config.csv";
    private const string DefaultContentType = "application/octet-stream";

    record RouteDefinition(string Method, string Route, int StatusCode, string ResponseFileName);
    
    public static IEndpointRouteBuilder UseFileSystemExtensionEndpoints(this IEndpointRouteBuilder builder)
    {
        var fileExtensionContentTypeProvider = new FileExtensionContentTypeProvider();

        var configuration = builder.ServiceProvider.GetRequiredService<FileSystemConfiguration>();
        var configurationFileName = Path.Combine(configuration.DataRootFolder, ConfigurationFileName);
        if (!File.Exists(configurationFileName))
        {
            throw new FileNotFoundException($"Could not find configuration file '{configurationFileName}'", configurationFileName);
        }

        using (var reader = new StreamReader(configurationFileName))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            if (!csv.Read() || !csv.ReadHeader())
            {
                throw new Exception($"Invalid configuration file {configurationFileName}");
            };

            foreach (var routeDefinition in csv.GetRecords<RouteDefinition>())
            {
                var contentsFileName = routeDefinition.ResponseFileName;
                if (!string.IsNullOrEmpty(contentsFileName))
                {
                    contentsFileName = Path.Combine(configuration.DataRootFolder, contentsFileName);
                    if (!File.Exists(contentsFileName))
                    {
                        Console.WriteLine($"Could not find {contentsFileName}");
                    }
                }

                if (!fileExtensionContentTypeProvider.TryGetContentType(contentsFileName, out var contentType))
                {
                    contentType = DefaultContentType;
                }

                switch (routeDefinition.Method)
                {
                    case "GET":
                        builder.MapGet(routeDefinition.Route, () =>
                        {
                            return Results.Text(File.ReadAllText(contentsFileName), contentType, null, routeDefinition.StatusCode);
                        });
                        break;
                    case "POST":
                        builder.MapPost(routeDefinition.Route, () =>
                        {
                            return Results.Text(File.ReadAllText(contentsFileName), contentType, null, routeDefinition.StatusCode);
                        });
                        break;
                    case "DELETE":
                        builder.MapPost(routeDefinition.Route, () =>
                        {
                            return Results.Ok();
                        });
                        break;
                    case "PUT":
                        builder.MapPut(routeDefinition.Route, () =>
                        {
                            return Results.Ok();
                        });
                        break;
                    default:
                        throw new NotImplementedException($"The method '{routeDefinition.Method}' has not been implemented");
                }
            }
        }

        return builder;
    }
}
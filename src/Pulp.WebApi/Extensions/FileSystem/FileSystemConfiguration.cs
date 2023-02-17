namespace Pulp.WebApi.Extensions.FileSystem;

public sealed class FileSystemConfiguration
{
    public string DataRootFolder { get; init; }

    internal FileSystemConfiguration(IConfiguration configuration)
    {
        var dataRootFolder = configuration.GetValue<string>("Extensions:FileSystem:DataRootFolder");
        if (dataRootFolder is null)
        {
            throw new Exception($"Configuration Extensions:FileSystem:DataRootFolder could not be found or is empty");
        }

        DataRootFolder = dataRootFolder;
    }
}

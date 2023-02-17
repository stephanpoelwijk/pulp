using Pulp.WebApi.Extensions.FileSystem;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
        .AddFileSystemEndpointServices(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseFileSystemExtensionEndpoints();
app.Run();

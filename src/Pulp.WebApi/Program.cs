using Pulp.WebApi.Extensions.FileSystem;
using Pulp.WebApi.Extensions.Generic;
using Pulp.WebApi.Extensions.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
       .AddEndpointsApiExplorer()
       .AddSwaggerGen()
       .AddFileSystemEndpointServices(builder.Configuration)
       .AddOpenApiExtensionServices(builder.Configuration)
       .AddGenericApiService();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseFileSystemExtensionEndpoints();
app.UseGenericApiEndpoints();
app.UseOpenApiEndpoints();
app.Run();

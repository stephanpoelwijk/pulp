using Pulp.WebApi.Extensions.FileSystem;
using Pulp.WebApi.Extensions.Generic;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
       .AddEndpointsApiExplorer()
       .AddSwaggerGen()
       .AddFileSystemEndpointServices(builder.Configuration)
       .AddGenericApiService(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseFileSystemExtensionEndpoints();
app.UseGenericApiEndpoints();
app.Run();

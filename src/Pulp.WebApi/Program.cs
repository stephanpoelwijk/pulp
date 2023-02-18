using Pulp.WebApi.Extensions.FileSystem;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
       .AddEndpointsApiExplorer()
       .AddSwaggerGen()
       .AddFileSystemEndpointServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseFileSystemExtensionEndpoints();
app.Run();

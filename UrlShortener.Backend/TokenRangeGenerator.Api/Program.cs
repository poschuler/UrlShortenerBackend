using System.Reflection;
using TokenRangeGenerator.Api;
using TokenRangeGenerator.Api.Shared.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();

builder.Services.AddOpenApi();

builder.Services.AddServices(builder.Configuration);

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

WebApplication app = builder.Build();

app.MapEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

//app.UseStatusCodePages();

app.UseExceptionHandler();

await app.RunAsync();

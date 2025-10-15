using TaskTracker.API;
using TaskTracker.API.Middleware;
using TaskTracker.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddInfrastructureServices(builder.Configuration, builder.Environment)
               .AddApplicationServices()
               .AddWebAppServices();              

var app = builder.Build();

app.UseMiddleware<ValidationExceptionMiddleware>();

app.UseSwaggerMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

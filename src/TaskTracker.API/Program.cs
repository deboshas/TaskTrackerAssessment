using Microsoft.OpenApi.Models;
using TaskTracker.API;
using TaskTracker.Application;
using TaskTracker.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddInfrastructureServices(builder.Configuration, builder.Environment)
               .AddApplicationServices()
               .AddWebAppServices();              

var app = builder.Build();

// Configure the HTTP request pipeline.  
app.UseMiddleware<ValidationExceptionMiddleware>();

app.UseSwaggerMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

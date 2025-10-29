using FluentValidation;
using MediatR;
using NewsAnalyzer.Api;
using NewsAnalyzer.Api.Middleware;
using NewsAnalyzer.Application.Common.Behaviors;
using NewsAnalyzer.Application.Health.Queries;
using NewsAnalyzer.Infrastructure;
using NewsAnalyzer.Infrastructure.Persistence;
using NewsAnalyzer.Infrastructure.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(GetHealthStatusQuery).Assembly);
});
builder.Services.AddValidatorsFromAssembly(typeof(GetHealthStatusQuery).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddInfrastructure(builder.Configuration);

// Exception middleware
builder.Services.AddTransient<ExceptionMiddleware>();


// MVC + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Create and migrate database if needed
await app.EnsureDbCreatedAndMigratedAsync();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.MapHub<NewsHub>("/hubs/news");

app.UseAuthorization();

app.MapControllers();

app.Run();

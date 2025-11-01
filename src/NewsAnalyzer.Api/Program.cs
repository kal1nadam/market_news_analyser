using FluentValidation;
using Hangfire;
using MediatR;
using NewsAnalyzer.Api;
using NewsAnalyzer.Api.Middleware;
using NewsAnalyzer.Application;
using NewsAnalyzer.Application.Common.Behaviors;
using NewsAnalyzer.Application.Common.Interfaces;
using NewsAnalyzer.Application.Health.Queries;
using NewsAnalyzer.Application.News.Commands;
using NewsAnalyzer.Infrastructure;
using NewsAnalyzer.Infrastructure.Common;
using NewsAnalyzer.Infrastructure.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(ImportNewsCommand).Assembly);
});
builder.Services.AddValidatorsFromAssembly(typeof(ImportNewsCommand).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Add typed http clients
builder.Services.AddHttpClients();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

// Exception middleware
builder.Services.AddTransient<ExceptionMiddleware>();


// MVC + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Add Hangfire dashboard
app.UseHangfireDashboard("/hangfire");

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

// Schedule background jobs
using (var scope = app.Services.CreateScope())
{
    var scheduler = scope.ServiceProvider.GetRequiredService<IBackgroundJobScheduler>();
    scheduler.ScheduleAll();
}

app.Run();

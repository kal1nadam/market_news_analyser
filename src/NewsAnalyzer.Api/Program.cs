using FluentValidation;
using MediatR;
using NewsAnalyzer.Api.Middleware;
using NewsAnalyzer.Application.Common.Behaviors;
using NewsAnalyzer.Application.Health.Queries;
using NewsAnalyzer.Infrastructure;

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

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();

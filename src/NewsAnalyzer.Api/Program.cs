using FluentValidation;
using NewsAnalyzer.Application.Health.Queries;
using NewsAnalyzer.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(GetHealthStatusQuery).Assembly);
});
builder.Services.AddValidatorsFromAssembly(typeof(GetHealthStatusQuery).Assembly);

builder.Services.AddInfrastructure(builder.Configuration);

// MVC + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();

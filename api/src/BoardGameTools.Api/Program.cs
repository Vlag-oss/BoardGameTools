using BoardGameTools.Application.Common.Options;
using BoardGameTools.Application.Extensions;
using BoardGameTools.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureService(builder.Configuration);

builder.Services
    .AddOptions<AuthOptions>()
    .Bind(builder.Configuration.GetSection("Auth"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services
    .AddOptions<TokenOptions>()
    .Bind(builder.Configuration.GetSection("Auth:Token"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services
    .AddOptions<BrevoOptions>()
    .Bind(builder.Configuration.GetSection("Brevo"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

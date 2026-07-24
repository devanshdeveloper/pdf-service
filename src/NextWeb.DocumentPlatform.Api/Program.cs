using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using NextWeb.DocumentPlatform.Application.Commands;
using NextWeb.DocumentPlatform.Application;
using NextWeb.DocumentPlatform.Engine;
using NextWeb.DocumentPlatform.Infrastructure;
using NextWeb.DocumentPlatform.Renderers.Templates;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = LicenseType.Community;

// Add services to the container.
builder.Services.AddControllers();

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RenderDocumentCommand).Assembly));

// Platform Services
builder.Services.AddSingleton<IDocumentRegistry, DocumentRegistry>();
builder.Services.AddSingleton<IDocumentRendererFactory, DocumentRendererFactory>();
builder.Services.AddHttpClient<IErpForwardingService, ErpForwardingService>();

// Renderers
builder.Services.AddTransient<IDocumentRenderer, VoucherStandardRenderer>();
builder.Services.AddTransient<IDocumentRenderer, GstStandardRenderer>();

// Health Checks
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Developer exception page is added by default in .NET 6+
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Custom endpoints for health and version
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true
});

app.MapHealthChecks("/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});

app.MapGet("/version", () => new { Version = "1.0.0" });

app.Run();

public partial class Program { }

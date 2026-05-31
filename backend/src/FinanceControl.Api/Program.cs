using FinanceControl.Api.Middlewares;
using FinanceControl.Api.Services;
using FinanceControl.Application.Common.Interfaces;
using FinanceControl.CrossCutting.DependencyInjection;
using FinanceControl.Data.Context;
using FinanceControl.Data.Seed;
using Microsoft.EntityFrameworkCore;
using Serilog;

// =========================================================================
// FINANCE CONTROL - API ENTRY POINT
// .NET 10 | Minimal API | Clean Architecture | CQRS
// =========================================================================

var builder = WebApplication.CreateBuilder(args);

// ---------- Serilog (fortemente tipado, configurado em CrossCutting) ----------
builder.Host.UseSerilog((context, services, configuration) =>
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

// ---------- Dependency Injection (separado em Application + Infrastructure) ----------
builder.Services.AddApplicationDependencies(builder.Configuration);
builder.Services.AddInfrastructureDependencies(builder.Configuration);

// ---------- API / Controllers / Swagger / Validation ----------
builder.Services.AddApiDependencies(builder.Configuration);

// CurrentUserService precisa do HttpContext (registrado pelo Infrastructure)
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

var app = builder.Build();

// ---------- Migrations + Seed automáticos em desenvolvimento ----------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FinanceControlDbContext>();
    await db.Database.MigrateAsync();
    await DatabaseSeeder.SeedAsync(db);
}

// ---------- Middleware Pipeline ----------
app.UseSerilogRequestLogging();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<CorrelationIdMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("FinanceControlPolicy");
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

// Permite usar esta classe em WebApplicationFactory<Program> nos testes de integração
public partial class Program { }

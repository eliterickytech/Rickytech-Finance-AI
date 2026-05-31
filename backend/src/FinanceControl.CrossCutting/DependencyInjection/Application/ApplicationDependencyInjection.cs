using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceControl.CrossCutting.DependencyInjection;

/// <summary>
/// Registra MediatR, AutoMapper, FluentValidation e behaviors da camada Application.
/// </summary>
public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplicationDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var appAssembly = Assembly.Load("FinanceControl.Application");

        // MediatR (CQRS)
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(appAssembly));

        // AutoMapper - todos os perfis da camada Application
        services.AddAutoMapper(appAssembly);

        // FluentValidation - todos os validators da camada Application
        services.AddValidatorsFromAssembly(appAssembly);

        return services;
    }
}

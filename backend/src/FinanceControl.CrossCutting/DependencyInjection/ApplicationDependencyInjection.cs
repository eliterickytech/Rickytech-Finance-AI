using System.Reflection;
using FinanceControl.Application.Common.Behaviors;
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

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(appAssembly);
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        // AutoMapper 14.x: a extensão DI deprecou o pacote separado e
        // mudou a assinatura para receber uma Action<IMapperConfigurationExpression>.
        //
        // CVE-2026-32933 (GHSA-rvv3-g6hj-g44x): a 14.x não recebeu patch (o
        // autor mudou licenciamento para comercial a partir da 15.x). A
        // mitigação oficial recomendada é cravar um MaxDepth defensivo em
        // todos os maps para inviabilizar o vetor de stack overflow.
        services.AddAutoMapper(cfg =>
        {
            cfg.AllowNullCollections = true;
            cfg.AddMaps(appAssembly);
            cfg.ForAllMaps((_, expr) => expr.MaxDepth(64));
        });

        services.AddValidatorsFromAssembly(appAssembly);

        return services;
    }
}

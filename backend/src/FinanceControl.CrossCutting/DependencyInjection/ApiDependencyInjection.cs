using System.Text;
using FinanceControl.CrossCutting.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace FinanceControl.CrossCutting.DependencyInjection;

public static class ApiDependencyInjection
{
    public static IServiceCollection AddApiDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        // ----- Swagger com suporte a Bearer JWT -----
        services.AddSwaggerGen(opts =>
        {
            opts.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Finance Control API",
                Version = "v1",
                Description = "API .NET 10 do Finance Control (Clean Architecture + CQRS).",
                Contact = new OpenApiContact { Name = "Rickytech", Email = "rickyteck@hotmail.com" }
            });

            opts.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Cole o JWT (sem o prefixo Bearer)."
            });
            opts.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()
                }
            });
        });

        services.AddHealthChecks();

        // ----- CORS -----
        services.AddCors(options =>
        {
            options.AddPolicy("FinanceControlPolicy", policy =>
                policy
                    .WithOrigins("http://localhost:3000", "http://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials());
        });

        // ----- JWT Authentication -----
        var jwt = new JwtOptions();
        configuration.GetSection(JwtOptions.SectionName).Bind(jwt);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opts =>
            {
                opts.RequireHttpsMetadata = false;
                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwt.Issuer,
                    ValidAudience = jwt.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(string.IsNullOrWhiteSpace(jwt.Secret)
                            ? "FinanceControlDefaultSecret_PleaseChange_32Bytes!"
                            : jwt.Secret))
                };
            });

        services.AddAuthorization();

        // ----- Rate limiting (FixedWindow) -----
        services.AddRateLimiter(o =>
        {
            o.AddFixedWindowLimiter("global", opts =>
            {
                opts.PermitLimit = 100;
                opts.Window = TimeSpan.FromMinutes(1);
            });
            o.AddFixedWindowLimiter("auth", opts =>
            {
                opts.PermitLimit = 10;
                opts.Window = TimeSpan.FromMinutes(1);
            });
        });

        return services;
    }
}

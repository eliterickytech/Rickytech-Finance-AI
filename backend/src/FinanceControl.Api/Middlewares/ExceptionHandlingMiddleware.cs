using System.Net;
using System.Text.Json;
using FinanceControl.Application.Common.Exceptions;
using FinanceControl.Application.Common.Models;

namespace FinanceControl.Api.Middlewares;

/// <summary>
/// Middleware global que captura exceções e retorna o envelope <see cref="ApiResponse{T}"/>.
/// </summary>
public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleAsync(context, ex);
        }
    }

    private async Task HandleAsync(HttpContext ctx, Exception ex)
    {
        var (status, message, errors) = ex switch
        {
            ValidationException ve   => (HttpStatusCode.BadRequest, "Validação falhou.", ve.Errors),
            NotFoundException nf     => (HttpStatusCode.NotFound, nf.Message, (IEnumerable<string>?)null),
            UnauthorizedException ua => (HttpStatusCode.Unauthorized, ua.Message, null),
            _ => (HttpStatusCode.InternalServerError, "Erro interno no servidor.", null)
        };

        _logger.LogError(ex, "Unhandled exception caught by middleware. Status: {Status}", status);

        ctx.Response.ContentType = "application/json";
        ctx.Response.StatusCode = (int)status;

        var payload = ApiResponse<object?>.FailureResult(message, errors);
        await ctx.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }
}

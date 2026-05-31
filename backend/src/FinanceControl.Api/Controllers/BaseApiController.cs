using FinanceControl.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinanceControl.Api.Controllers;

/// <summary>
/// Controller base que padroniza todos os responses da API (envelope ApiResponse&lt;T&gt;)
/// e expõe um <see cref="IMediator"/> resolvido sob demanda.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public abstract class BaseApiController : ControllerBase
{
    private IMediator? _mediator;

    protected IMediator Mediator =>
        _mediator ??= HttpContext.RequestServices.GetService<IMediator>()
            ?? throw new InvalidOperationException("IMediator not registered.");

    protected IActionResult OkResponse<T>(T data, string? message = null)
        => Ok(ApiResponse<T>.SuccessResult(data, message));

    protected IActionResult CreatedResponse<T>(string location, T data, string? message = null)
        => Created(location, ApiResponse<T>.SuccessResult(data, message));

    protected IActionResult NoContentResponse(string? message = null)
        => Ok(ApiResponse<object?>.SuccessResult(null, message ?? "Operação realizada com sucesso."));

    protected IActionResult BadRequestResponse(string message, IEnumerable<string>? errors = null)
        => BadRequest(ApiResponse<object?>.FailureResult(message, errors));

    protected IActionResult NotFoundResponse(string message)
        => NotFound(ApiResponse<object?>.FailureResult(message));
}

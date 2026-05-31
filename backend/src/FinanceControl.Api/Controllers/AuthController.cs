using FinanceControl.Application.Features.Auth.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceControl.Api.Controllers;

[Route("api/v1/auth")]
[AllowAnonymous]
public sealed class AuthController : BaseApiController
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand cmd, CancellationToken ct)
        => OkResponse(await Mediator.Send(cmd, ct), "Login realizado com sucesso.");
}

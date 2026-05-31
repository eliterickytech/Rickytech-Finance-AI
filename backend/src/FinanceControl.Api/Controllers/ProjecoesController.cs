using FinanceControl.Application.Features.Projections.Commands;
using Microsoft.AspNetCore.Mvc;

namespace FinanceControl.Api.Controllers;

[Route("api/v1/projecoes")]
public sealed class ProjecoesController : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> Run([FromBody] RunProjectionCommand cmd, CancellationToken ct)
        => OkResponse(await Mediator.Send(cmd, ct), "Projeção gerada com sucesso.");
}

using FinanceControl.Application.Features.Integrations.Commands.Binance;
using FinanceControl.Application.Features.Integrations.Queries;
using Microsoft.AspNetCore.Mvc;

namespace FinanceControl.Api.Controllers;

[Route("api/v1/integracoes/binance")]
public sealed class IntegracoesBinanceController : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> SaveCredentials([FromBody] SaveBinanceCredentialsCommand cmd, CancellationToken ct)
        => OkResponse(await Mediator.Send(cmd, ct), "Credenciais Binance salvas com sucesso.");

    [HttpPost("sync")]
    public async Task<IActionResult> Sync(CancellationToken ct)
        => OkResponse(await Mediator.Send(new SyncBinanceCommand(), ct), "Sincronização concluída.");

    [HttpGet("status")]
    public async Task<IActionResult> Status(CancellationToken ct)
        => OkResponse(await Mediator.Send(new GetBinanceStatusQuery(), ct));
}

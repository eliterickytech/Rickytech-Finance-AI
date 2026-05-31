using FinanceControl.Application.Features.Integrations.Commands.OpenFinance;
using FinanceControl.Application.Features.Integrations.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FinanceControl.Api.Controllers;

[Route("api/v1/integracoes/openfinance")]
public sealed class IntegracoesOpenFinanceController : BaseApiController
{
    [HttpPost("consentir")]
    public async Task<IActionResult> StartConsent([FromBody] StartConsentDto body, CancellationToken ct)
    {
        var dto = await Mediator.Send(new StartConsentCommand(body.Cpf, body.BankCode), ct);
        return OkResponse(dto, "Consentimento iniciado.");
    }

    [HttpGet("callback")]
    public async Task<IActionResult> Callback([FromQuery] string code, [FromQuery] string state, CancellationToken ct)
    {
        await Mediator.Send(new HandleCallbackCommand(code, state), ct);
        return OkResponse(new { code, state, status = "connected" }, "Open Finance conectado com sucesso.");
    }

    [HttpPost("sync")]
    public async Task<IActionResult> Sync(CancellationToken ct)
        => OkResponse(await Mediator.Send(new SyncOpenFinanceCommand(), ct), "Sincronização concluída.");
}

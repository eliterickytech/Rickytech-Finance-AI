using FinanceControl.Application.Features.Investments.Commands;
using FinanceControl.Application.Features.Investments.Dtos;
using FinanceControl.Application.Features.Investments.Queries;
using FinanceControl.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FinanceControl.Api.Controllers;

[Route("api/v1/investimentos")]
public sealed class InvestimentosController : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateInvestmentCommand cmd, CancellationToken ct)
    {
        var dto = await Mediator.Send(cmd, ct);
        return CreatedResponse($"/api/v1/investimentos/{dto.Id}", dto, "Investimento criado com sucesso.");
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        => OkResponse(await Mediator.Send(new GetInvestmentByIdQuery(id), ct));

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] InvestmentType? type, [FromQuery] Guid? bankId, CancellationToken ct)
        => OkResponse(await Mediator.Send(new GetInvestmentsQuery(type, bankId), ct));

    [HttpPost("{id:guid}/operacoes")]
    public async Task<IActionResult> RegisterOperation(Guid id, [FromBody] RegisterOperationDto body, CancellationToken ct)
    {
        var cmd = new RegisterOperationCommand(id, body.Side, body.Quantity, body.Price, body.Fee, body.ExecutedAt);
        return OkResponse(await Mediator.Send(cmd, ct), "Operação registrada com sucesso.");
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await Mediator.Send(new DeleteInvestmentCommand(id), ct);
        return NoContentResponse("Investimento removido.");
    }

    [HttpGet("resumo")]
    public async Task<IActionResult> Summary(CancellationToken ct)
        => OkResponse(await Mediator.Send(new GetPortfolioSummaryQuery(), ct));
}

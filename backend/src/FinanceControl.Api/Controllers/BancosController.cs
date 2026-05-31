using FinanceControl.Application.Features.Banks.Commands.CreateBank;
using FinanceControl.Application.Features.Banks.Commands.DeleteBank;
using FinanceControl.Application.Features.Banks.Commands.UpdateBank;
using FinanceControl.Application.Features.Banks.Dtos;
using FinanceControl.Application.Features.Banks.Queries.GetAllBanks;
using FinanceControl.Application.Features.Banks.Queries.GetBankById;
using FinanceControl.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FinanceControl.Api.Controllers;

[Route("api/v1/bancos")]
public sealed class BancosController : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBankCommand cmd, CancellationToken ct)
    {
        var dto = await Mediator.Send(cmd, ct);
        return CreatedResponse($"/api/v1/bancos/{dto.Id}", dto, "Banco criado com sucesso.");
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        => OkResponse(await Mediator.Send(new GetBankByIdQuery(id), ct));

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] BankAccountType? type, [FromQuery] string? currency, [FromQuery] bool? active,
        CancellationToken ct)
        => OkResponse(await Mediator.Send(new GetAllBanksQuery(type, currency, active), ct));

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBankDto body, CancellationToken ct)
        => OkResponse(await Mediator.Send(new UpdateBankCommand(id, body.Nickname, body.Active), ct),
            "Banco atualizado com sucesso.");

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await Mediator.Send(new DeleteBankCommand(id), ct);
        return NoContentResponse("Banco removido com sucesso.");
    }
}

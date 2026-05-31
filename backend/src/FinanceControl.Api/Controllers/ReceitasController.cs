using FinanceControl.Application.Features.Incomes.Commands.CreateIncome;
using FinanceControl.Application.Features.Incomes.Commands.DeleteIncome;
using FinanceControl.Application.Features.Incomes.Commands.UpdateIncome;
using FinanceControl.Application.Features.Incomes.Dtos;
using FinanceControl.Application.Features.Incomes.Queries.GetIncomes;
using Microsoft.AspNetCore.Mvc;

namespace FinanceControl.Api.Controllers;

[Route("api/v1/receitas")]
public sealed class ReceitasController : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateIncomeCommand cmd, CancellationToken ct)
    {
        var dto = await Mediator.Send(cmd, ct);
        return CreatedResponse($"/api/v1/receitas/{dto.Id}", dto, "Receita criada com sucesso.");
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        => OkResponse(await Mediator.Send(new GetIncomeByIdQuery(id), ct));

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetIncomesQuery query, CancellationToken ct)
        => OkResponse(await Mediator.Send(query, ct));

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateIncomeDto body, CancellationToken ct)
    {
        var cmd = new UpdateIncomeCommand(id, body.Description, body.Amount, body.Date,
            body.CategoryId, body.BankId, body.Tags, body.Recurrence, body.RecurrenceEnd);
        return OkResponse(await Mediator.Send(cmd, ct), "Receita atualizada com sucesso.");
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await Mediator.Send(new DeleteIncomeCommand(id), ct);
        return NoContentResponse("Receita removida com sucesso.");
    }
}

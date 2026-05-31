using FinanceControl.Application.Features.Expenses.Commands;
using FinanceControl.Application.Features.Expenses.Dtos;
using FinanceControl.Application.Features.Expenses.Queries;
using Microsoft.AspNetCore.Mvc;

namespace FinanceControl.Api.Controllers;

[Route("api/v1/despesas")]
public sealed class DespesasController : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateExpenseCommand cmd, CancellationToken ct)
    {
        var dto = await Mediator.Send(cmd, ct);
        return CreatedResponse($"/api/v1/despesas/{dto.Id}", dto, "Despesa criada com sucesso.");
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        => OkResponse(await Mediator.Send(new GetExpenseByIdQuery(id), ct));

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetExpensesQuery query, CancellationToken ct)
        => OkResponse(await Mediator.Send(query, ct));

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateExpenseDto body, CancellationToken ct)
    {
        var cmd = new UpdateExpenseCommand(id, body.Description, body.Amount, body.Date,
            body.CategoryId, body.BankId, body.PaymentMethod, body.Tags, body.Recurrence, body.RecurrenceEnd);
        return OkResponse(await Mediator.Send(cmd, ct), "Despesa atualizada com sucesso.");
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await Mediator.Send(new DeleteExpenseCommand(id), ct);
        return NoContentResponse("Despesa removida com sucesso.");
    }
}

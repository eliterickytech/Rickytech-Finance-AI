using FinanceControl.Application.Features.News.Queries;
using FinanceControl.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FinanceControl.Api.Controllers;

[Route("api/v1/noticias")]
public sealed class NoticiasController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] NewsCategory? category,
        [FromQuery] string? tag,
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
        => OkResponse(await Mediator.Send(new GetNewsQuery(category, tag, search, page, pageSize), ct));
}

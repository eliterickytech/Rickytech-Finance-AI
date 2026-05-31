using FinanceControl.Application.Features.Categories.Commands.CreateCategory;
using FinanceControl.Application.Features.Categories.Commands.DeleteCategory;
using FinanceControl.Application.Features.Categories.Commands.UpdateCategory;
using FinanceControl.Application.Features.Categories.Dtos;
using FinanceControl.Application.Features.Categories.Queries.GetAllCategories;
using FinanceControl.Application.Features.Categories.Queries.GetCategoryById;
using FinanceControl.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FinanceControl.Api.Controllers;

[Route("api/v1/categorias")]
public sealed class CategoriasController : BaseApiController
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateCategoryCommand cmd, CancellationToken ct)
    {
        var dto = await Mediator.Send(cmd, ct);
        return CreatedResponse($"/api/v1/categorias/{dto.Id}", dto, "Categoria criada com sucesso.");
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        => OkResponse(await Mediator.Send(new GetCategoryByIdQuery(id), ct));

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<CategoryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] CategoryType? type, [FromQuery] bool? active, [FromQuery] string? search,
        CancellationToken ct)
        => OkResponse(await Mediator.Send(new GetAllCategoriesQuery(type, active, search), ct));

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryDto body, CancellationToken ct)
    {
        var cmd = new UpdateCategoryCommand(id, body.Name, body.Type, body.Color, body.Icon, body.Active);
        return OkResponse(await Mediator.Send(cmd, ct), "Categoria atualizada com sucesso.");
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await Mediator.Send(new DeleteCategoryCommand(id), ct);
        return NoContentResponse("Categoria removida com sucesso.");
    }
}

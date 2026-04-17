using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Api.Models;
using TaskManagement.Api.Services;

namespace TaskManagement.Api.Controllers;

/// <summary>
/// Base class for controllers that provide CRUD operations.
/// </summary>
public abstract class BaseCrudController<TEntity, TDto, TCreateDto, TUpdateDto> : ControllerBase
    where TEntity : class, IBaseEntity
{
    protected readonly ICrudService<TEntity, TDto, TCreateDto, TUpdateDto> _service;

    protected BaseCrudController(ICrudService<TEntity, TDto, TCreateDto, TUpdateDto> service)
    {
        _service = service;
    }

    [HttpGet]
    public virtual async Task<ActionResult<IEnumerable<TDto>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public virtual async Task<ActionResult<TDto>> GetById(int id)
    {
        return Ok(await _service.GetByIdAsync(id));
    }

    [HttpPost]
    public virtual async Task<ActionResult<TDto>> Create(TCreateDto createDto)
    {
        var result = await _service.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = (result as dynamic).Id }, result);
    }

    [HttpPost("batch")]
    public virtual async Task<ActionResult> CreateBatch(IEnumerable<TCreateDto> createDtos)
    {
        await _service.CreateBatchAsync(createDtos);
        return NoContent();
    }

    [HttpPut("{id}")]
    public virtual async Task<ActionResult<TDto>> Update(int id, TUpdateDto updateDto)
    {
        return Ok(await _service.UpdateAsync(id, updateDto));
    }

    [HttpPatch("{id}")]
    public virtual async Task<ActionResult<TDto>> Patch(int id, [FromBody] JsonPatchDocument<TEntity> patchDocument)
    {
        return Ok(await _service.PatchAsync(id, patchDocument));
    }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}

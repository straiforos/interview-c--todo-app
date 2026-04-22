using Microsoft.AspNetCore.Mvc;
using TaskManagement.Api.DTOs;
using TaskManagement.Api.Models;
using TaskManagement.Api.Services;
using TaskManagement.Api.Interceptors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;

namespace TaskManagement.Api.Controllers;

/// <summary>
/// Controller for managing task items.
/// </summary>
[Authorize]
[ApiExceptionFilter]
[ApiController]
[Route("api/[controller]")]
public class TasksController : BaseCrudController<TaskItem, TaskDto, CreateTaskDto, UpdateTaskDto, ITaskService>
{
    public TasksController(ITaskService service) : base(service)
    {
    }

    [HttpGet]
    [HasPermission(AppPermission.TasksRead)]
    public override async Task<ActionResult<IEnumerable<TaskDto>>> GetAll()
    {
        // We override GetAll to return Summary DTOs instead of full TaskDtos
        // This demonstrates the DTO pattern: List vs Detail
        var tasks = await _service.GetAllSummariesAsync();
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    [HasPermission(AppPermission.TasksRead)]
    public override Task<ActionResult<TaskDto>> GetById(int id) => base.GetById(id);

    [HttpPost]
    [HasPermission(AppPermission.TasksCreate)]
    public override Task<ActionResult<TaskDto>> Create(CreateTaskDto createDto) => base.Create(createDto);

    [HttpPut("{id}")]
    [HasPermission(AppPermission.TasksUpdate)]
    public override Task<ActionResult<TaskDto>> Update(int id, UpdateTaskDto updateDto) => base.Update(id, updateDto);

    [HttpPatch("{id}")]
    [HasPermission(AppPermission.TasksUpdate)]
    public override Task<ActionResult<TaskDto>> Patch(int id, [FromBody] JsonPatchDocument<TaskItem> patchDocument) => base.Patch(id, patchDocument);

    [HttpDelete("{id}")]
    [HasPermission(AppPermission.TasksDelete)]
    public override Task<IActionResult> Delete(int id) => base.Delete(id);
}

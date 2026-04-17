using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Api.DTOs;
using TaskManagement.Api.Models;
using TaskManagement.Api.Services;

namespace TaskManagement.Api.Controllers;

/// <summary>
/// Controller for managing task items.
/// </summary>
[Authorize]
public class TasksController : BaseCrudController<TaskItem, TaskDto, CreateTaskDto, UpdateTaskDto>
{
    public TasksController(ITaskService service) : base(service)
    {
    }
}

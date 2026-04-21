using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Api.Data;
using TaskManagement.Api.DTOs;
using TaskManagement.Api.Interceptors;
using TaskManagement.Api.Models;

namespace TaskManagement.Api.Services;

public interface ITaskService : ICrudService<TaskItem, TaskDto, CreateTaskDto, UpdateTaskDto>
{
    Task<IEnumerable<TaskSummaryDto>> GetAllSummariesAsync();
}

public class TaskService : CrudService<TaskItem, TaskDto, CreateTaskDto, UpdateTaskDto>, ITaskService
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public TaskService(
        ICrudRepository<TaskItem> repository,
        IMapper mapper,
        AppDbContext context, 
        ICurrentUserService currentUserService) : base(repository, mapper)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<TaskSummaryDto>> GetAllSummariesAsync()
    {
        var userId = _currentUserService.UserId;
        var tasks = await _context.Tasks
            .Where(t => t.CreatorId == userId || t.AssigneeId == userId)
            .ToListAsync();
        return _mapper.Map<IEnumerable<TaskSummaryDto>>(tasks);
    }

    public override async Task<IEnumerable<TaskDto>> GetAllAsync()
    {
        var userId = _currentUserService.UserId;
        var tasks = await _context.Tasks
            .Where(t => t.CreatorId == userId || t.AssigneeId == userId)
            .ToListAsync();
        return _mapper.Map<IEnumerable<TaskDto>>(tasks);
    }

    public override async Task<TaskDto?> GetByIdAsync(int id)
    {
        var userId = _currentUserService.UserId;
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == id && (t.CreatorId == userId || t.AssigneeId == userId));
        
        if (task == null) throw new NotFoundException($"Task with ID {id} not found or access denied.");
        
        return _mapper.Map<TaskDto>(task);
    }

    public override async Task<TaskDto> CreateAsync(CreateTaskDto dto)
    {
        var task = _mapper.Map<TaskItem>(dto);
        task.CreatorId = _currentUserService.UserId!;

        await _repository.AddAsync(task);
        return _mapper.Map<TaskDto>(task);
    }

    public override async Task<TaskDto?> UpdateAsync(int id, UpdateTaskDto dto)
    {
        var userId = _currentUserService.UserId;
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == id && (t.CreatorId == userId || t.AssigneeId == userId));

        if (task == null) throw new NotFoundException($"Task with ID {id} not found or access denied.");

        _mapper.Map(dto, task);
        task.Id = id;
        task.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(task);
        return _mapper.Map<TaskDto>(task);
    }

    public override async Task<bool> DeleteAsync(int id)
    {
        var userId = _currentUserService.UserId;
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == id && (t.CreatorId == userId || t.AssigneeId == userId));

        if (task == null) throw new NotFoundException($"Task with ID {id} not found or access denied.");

        await _repository.DeleteAsync(id);
        return true;
    }
}

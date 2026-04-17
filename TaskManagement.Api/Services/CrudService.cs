using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using TaskManagement.Api.Data;
using TaskManagement.Api.Interceptors;
using TaskManagement.Api.Models;

namespace TaskManagement.Api.Services;

/// <summary>
/// Generic service implementation for CRUD operations.
/// Handles DTO mapping and delegates data access to the repository.
/// </summary>
public class CrudService<TEntity, TDto, TCreateDto, TUpdateDto> : ICrudService<TEntity, TDto, TCreateDto, TUpdateDto>
    where TEntity : class, IBaseEntity
{
    protected readonly ICrudRepository<TEntity> _repository;
    protected readonly IMapper _mapper;

    public CrudService(ICrudRepository<TEntity> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public virtual async Task<TDto> CreateAsync(TCreateDto dto)
    {
        var entity = _mapper.Map<TEntity>(dto);
        await _repository.AddAsync(entity);
        return _mapper.Map<TDto>(entity);
    }

    public virtual async Task CreateBatchAsync(IEnumerable<TCreateDto> dtos)
    {
        var entities = _mapper.Map<IEnumerable<TEntity>>(dtos);
        await _repository.AddAllAsync(entities);
    }

    public virtual async Task<TDto?> GetByIdAsync(int id)
    {
        var entity = await _repository.FindByIdAsync(id);
        if (entity == null) throw new NotFoundException($"{typeof(TEntity).Name} with ID {id} not found.");
        return _mapper.Map<TDto>(entity);
    }

    public virtual async Task<IEnumerable<TDto>> GetAllAsync()
    {
        var entities = await _repository.FindAllAsync();
        return _mapper.Map<IEnumerable<TDto>>(entities);
    }

    public virtual async Task<TDto?> UpdateAsync(int id, TUpdateDto dto)
    {
        var entity = await _repository.FindByIdAsync(id);
        if (entity == null) throw new NotFoundException($"{typeof(TEntity).Name} with ID {id} not found.");

        _mapper.Map(dto, entity);
        entity.Id = id;
        await _repository.UpdateAsync(entity);
        return _mapper.Map<TDto>(entity);
    }

    public virtual async Task<TDto?> PatchAsync(int id, JsonPatchDocument<TEntity> patchDocument)
    {
        var entity = await _repository.FindByIdAsync(id);
        if (entity == null) throw new NotFoundException($"{typeof(TEntity).Name} with ID {id} not found.");

        await _repository.PatchAsync(id, patchDocument);
        return _mapper.Map<TDto>(entity);
    }

    public virtual async Task<bool> DeleteAsync(int id)
    {
        var entity = await _repository.FindByIdAsync(id);
        if (entity == null) throw new NotFoundException($"{typeof(TEntity).Name} with ID {id} not found.");

        await _repository.DeleteAsync(id);
        return true;
    }
}

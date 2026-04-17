using Microsoft.AspNetCore.JsonPatch;

namespace TaskManagement.Api.Services;

public interface ICreateService<TEntity, TDto, TCreateDto> where TEntity : class
{
    Task<TDto> CreateAsync(TCreateDto dto);
    Task CreateBatchAsync(IEnumerable<TCreateDto> dtos);
}

public interface IReadService<TEntity, TDto> where TEntity : class
{
    Task<TDto?> GetByIdAsync(int id);
    Task<IEnumerable<TDto>> GetAllAsync();
}

public interface IUpdateService<TEntity, TDto, TUpdateDto> where TEntity : class
{
    Task<TDto?> UpdateAsync(int id, TUpdateDto dto);
    Task<TDto?> PatchAsync(int id, JsonPatchDocument<TEntity> patchDocument);
}

public interface IDeleteService<TEntity> where TEntity : class
{
    Task<bool> DeleteAsync(int id);
}

public interface ICrudService<TEntity, TDto, TCreateDto, TUpdateDto> : 
    ICreateService<TEntity, TDto, TCreateDto>, 
    IReadService<TEntity, TDto>, 
    IUpdateService<TEntity, TDto, TUpdateDto>, 
    IDeleteService<TEntity> 
    where TEntity : class
{
}

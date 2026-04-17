using Microsoft.AspNetCore.JsonPatch;

namespace TaskManagement.Api.Data;

public interface ICreateRepository<TEntity> where TEntity : class
{
    Task<TEntity> AddAsync(TEntity entity);
    Task AddAllAsync(IEnumerable<TEntity> entities);
}

public interface IReadRepository<TEntity> where TEntity : class
{
    Task<TEntity?> FindByIdAsync(int id);
    Task<IEnumerable<TEntity>> FindAllAsync();
}

public interface IUpdateRepository<TEntity> where TEntity : class
{
    Task UpdateAsync(TEntity entity);
    Task PatchAsync(int id, JsonPatchDocument<TEntity> patchDocument);
}

public interface IDeleteRepository<TEntity> where TEntity : class
{
    Task DeleteAsync(int id);
}

public interface ICrudRepository<TEntity> : 
    ICreateRepository<TEntity>, 
    IReadRepository<TEntity>, 
    IUpdateRepository<TEntity>, 
    IDeleteRepository<TEntity> 
    where TEntity : class
{
}

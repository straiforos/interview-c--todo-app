using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Api.Models;

namespace TaskManagement.Api.Data;

/// <summary>
/// Generic Entity Framework implementation of the CRUD repository.
/// Provides base data access logic including support for Soft Delete and JSON Patch.
/// </summary>
/// <typeparam name="TEntity">The type of the domain entity.</typeparam>
public class EfRepository<TEntity> : ICrudRepository<TEntity> where TEntity : class, IBaseEntity
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public EfRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task AddAllAsync(IEnumerable<TEntity> entities)
    {
        await _dbSet.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }

    public virtual async Task<TEntity?> FindByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<TEntity>> FindAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public virtual async Task PatchAsync(int id, JsonPatchDocument<TEntity> patchDocument)
    {
        var entity = await FindByIdAsync(id);
        if (entity == null) return;

        patchDocument.ApplyTo(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await FindByIdAsync(id);
        if (entity == null) return;

        if (entity is ISoftDeletable softDeletable)
        {
            softDeletable.IsDeleted = true;
            softDeletable.DeletedAt = DateTime.UtcNow;
            _context.Entry(entity).State = EntityState.Modified;
        }
        else
        {
            _dbSet.Remove(entity);
        }

        await _context.SaveChangesAsync();
    }
}

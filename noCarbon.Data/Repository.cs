using Microsoft.EntityFrameworkCore;
using noCarbon.Core;
using noCarbon.Data.Configurations;

namespace noCarbon.Data;
/// <summary>
/// Represents the Entity Framework repository
/// </summary>
/// <typeparam name="TEntity">Entity type</typeparam>
public partial class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    #region Fields

    private readonly AppDbContext _context;
    private DbSet<TEntity> _entities;
    #endregion

    #region Ctor

    public Repository(AppDbContext context)
    {
        this._context = context;
    }

    #endregion

    #region Utilities

    /// <summary>
    /// Rollback of entity changes and return full error message
    /// </summary>
    /// <param name="exception">Exception</param>
    /// <returns>Error message</returns>
    protected string GetFullErrorTextAndRollbackEntityChanges(DbUpdateException exception)
    {
        //rollback entity changes
        if (_context is DbContext dbContext)
        {
            var entries = dbContext.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified).ToList();

            entries.ForEach(entry =>
            {
                try
                {
                    entry.State = EntityState.Unchanged;
                }
                catch (InvalidOperationException)
                {
                    // ignored
                }
            });
        }

        try
        {
            _context.SaveChanges();
            return exception.ToString();
        }
        catch (Exception ex)
        {
            //if after the rollback of changes the context is still not saving,
            //return the full text of the exception that occurred when saving
            return ex.ToString();
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Get entity by identifier
    /// </summary>
    /// <param name="id">Identifier</param>
    /// <returns>Entity</returns>
    public virtual TEntity GetById(object id)
    {
        try
        {
            return _entities.Find(id);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Get entity by identifier
    /// </summary>
    /// <param name="id">Identifier</param>
    /// <returns>Entity</returns>
    public virtual async Task<TEntity> GetByIdAsync(object id)
    {
        try
        {
            var result = await _entities.FindAsync(id);
            return result;
        }
        catch 
        {
            throw;
        }
    }

    /// <summary>
    /// Insert entity
    /// </summary>
    /// <param name="entity">Entity</param>
    public virtual void Insert(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        try
        {
            _entities.Add(entity);
            _context.SaveChanges();
        }
        catch (DbUpdateException exception)
        {
            //ensure that the detailed error text is saved in the Log
            throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
        }
    }

    /// <summary>
    /// Insert entity
    /// </summary>
    /// <param name="entity">Entity</param>
    public virtual async Task<TEntity> InsertAsync(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        try
        {
            _entities.Add(entity);
            _context.SaveChanges();
            return entity;
        }
        catch (DbUpdateException exception)
        {
            //ensure that the detailed error text is saved in the Log
            throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
        }
    }

    /// <summary>
    /// Insert entities
    /// </summary>
    /// <param name="entities">_entities</param>
    public virtual void Insert(IEnumerable<TEntity> entities)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));

        try
        {
            _entities.AddRange(entities);
            _context.SaveChanges();
        }
        catch (DbUpdateException exception)
        {
            //ensure that the detailed error text is saved in the Log
            throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
        }
    }

    /// <summary>
    /// Insert entities
    /// </summary>
    /// <param name="entities">_entities</param>
    public virtual async Task InsertAsync(IEnumerable<TEntity> entities)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));

        try
        {
            _entities.AddRange(entities);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
        {
            //ensure that the detailed error text is saved in the Log
            throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
        }
    }

    /// <summary>
    /// Update entity
    /// </summary>
    /// <param name="entity">Entity</param>
    public virtual void Update(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        try
        {
            _entities.Update(entity);
            _context.SaveChanges();
        }
        catch (DbUpdateException exception)
        {
            //ensure that the detailed error text is saved in the Log
            throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
        }
    }

    /// <summary>
    /// Update entity
    /// </summary>
    /// <param name="entity">Entity</param>
    public virtual async Task<TEntity> UpdateAsync(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        try
        {
            _entities.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        catch (DbUpdateException exception)
        {
            //ensure that the detailed error text is saved in the Log
            throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
        }
    }

    /// <summary>
    /// Update entities
    /// </summary>
    /// <param name="entities">_entities</param>
    public virtual void Update(IEnumerable<TEntity> entities)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));

        try
        {
            _entities.UpdateRange(entities);
            _context.SaveChanges();
        }
        catch (DbUpdateException exception)
        {
            //ensure that the detailed error text is saved in the Log
            throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
        }
    }

    /// <summary>
    /// Update entities
    /// </summary>
    /// <param name="entities">_entities</param>
    public virtual async Task UpdateAsync(IEnumerable<TEntity> entities)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));

        try
        {
            _entities.UpdateRange(entities);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
        {
            //ensure that the detailed error text is saved in the Log
            throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
        }
    }

    /// <summary>
    /// Delete entity
    /// </summary>
    /// <param name="entity">Entity</param>
    public virtual void Delete(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        try
        {
            switch (entity)
            {
                case null:
                    throw new ArgumentNullException(nameof(entity));
                case ISoftDeletedEntity softDeletedEntity:
                    softDeletedEntity.Deleted = true;
                    softDeletedEntity.DeletedDate = DateTime.UtcNow;
                    _entities.Update(entity);
                    break;
                default:
                    _entities.Remove(entity);
                    break;
            }
            _context.SaveChanges();
        }
        catch (DbUpdateException exception)
        {
            //ensure that the detailed error text is saved in the Log
            throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
        }
    }

    /// <summary>
    /// Delete entity
    /// </summary>
    /// <param name="entity">Entity</param>
    public virtual async Task DeleteAsync(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        try
        {
            switch (entity)
            {
                case null:
                    throw new ArgumentNullException(nameof(entity));
                case ISoftDeletedEntity softDeletedEntity:
                    softDeletedEntity.Deleted = true;
                    softDeletedEntity.DeletedDate = DateTime.UtcNow;
                    _entities.Update(entity);
                    break;
                default:
                    _entities.Remove(entity);
                    break;
            }
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
        {
            //ensure that the detailed error text is saved in the Log
            throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
        }
    }

    /// <summary>
    /// Delete entities
    /// </summary>
    /// <param name="entities">_entities</param>
    public virtual void Delete(IEnumerable<TEntity> entities)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));

        try
        {
            _entities.RemoveRange(entities);
            _context.SaveChanges();
        }
        catch (DbUpdateException exception)
        {
            //ensure that the detailed error text is saved in the Log
            throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
        }
    }

    /// <summary>
    /// Delete entities
    /// </summary>
    /// <param name="entities">_entities</param>
    public virtual async Task DeleteAsync(IEnumerable<TEntity> entities)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));

        try
        {
            _entities.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
        {
            //ensure that the detailed error text is saved in the Log
            throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
        }
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets a table
    /// </summary>
    public virtual IQueryable<TEntity> Table => Entities;

    /// <summary>
    /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
    /// </summary>
    public virtual IQueryable<TEntity> TableNoTracking => Entities.AsNoTracking();

    /// <summary>
    /// Gets an entity set
    /// </summary>
    protected virtual DbSet<TEntity> Entities
    {
        get
        {
            if (_entities == null)
                _entities = _context.Set<TEntity>();

            return _entities;
        }
    }

    #endregion
}
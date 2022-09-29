using noCarbon.Core.Configurations;
using noCarbon.Core.Domain;
using noCarbon.Core.Dtos;
using noCarbon.Core.Exceptions;
using noCarbon.Data;

namespace noCarbon.Services.Action;

/// <summary>
/// Action service
/// </summary>
public partial class ActionsService : IActionsService
{
    #region Fields
    private readonly IRepository<Actions> _ActionsRepository;
    #endregion
    #region Ctor
    /// <summary>
    /// Ctor Action service
    /// </summary>
    /// <param name="ActionsRepository">Action repository</param>
    public ActionsService(IRepository<Actions> ActionsRepository)
    {
        _ActionsRepository = ActionsRepository;
    }
    #endregion
    #region Methods
    /// <summary>
    /// Get all action list by Category
    /// </summary>
    /// <param name="CategoryId">action category if CategoryId null, you'll get all categories</param>
    /// <returns>action list</returns>
    public async Task<IList<ActionsDto>> GetAll(int? CategoryId = null)
    {
        var result = _ActionsRepository.TableNoTracking.Select(a => new ActionsDto() { Id = a.Id, Name = a.Name, Description = a.Description, CategoryId = a.CategoryId, Points = a.Points, ReducedCarb = a.ReducedCarb }).AsQueryable();
        if (CategoryId.HasValue && CategoryId != 0)
            return await result.Where(x => x.CategoryId == CategoryId).ToListAsync();
        return await result.ToListAsync();
    }
    /// <summary>
    /// Get a specific action by id
    /// </summary>
    /// <param name="id">action identifier</param>
    /// <returns>action</returns>
    public async Task<ActionsDto> GetById(int id)
    {
        if (id == 0)
            throw new ArgumentNullException(nameof(id));
        var action = await _ActionsRepository.GetByIdAsync(id);
        if (action == null)
            throw new EntityNotFoundException(string.Format("cannot find an entity {0} with the identifier {1} ", typeof(Actions), id));
        return new ActionsDto() { Id = action.Id, Name = action.Name, Description = action.Description, CategoryId = action.CategoryId, Points = action.Points, ReducedCarb = action.ReducedCarb };
    }
    /// <summary>
    /// add new action
    /// </summary>
    /// <param name="dto">action info</param>
    /// <returns></returns>
    public async Task Add(ActionsDto dto)
    {
        try
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            await _ActionsRepository.InsertAsync(new Actions
            {
                CategoryId = dto.CategoryId,
                Name = dto.Name,
                Description = dto.Description,
                ReducedCarb = dto.ReducedCarb,
                Points = dto.Points,
            });
            throw new SuccessfulOperationException("Successful operation");
        }
        catch
        {
            throw;
        }
    }
    /// <summary>
    /// Delete an action by identifier
    /// </summary>
    /// <param name="id">action identifier</param>
    /// <returns></returns>
    public async Task Delete(int id)
    {
        try
        {
            if (id == 0)
                throw new ArgumentNullException(nameof(id));
            var Actions = _ActionsRepository.GetById(id);
            await _ActionsRepository.DeleteAsync(Actions);
            throw new SuccessfulOperationException("Successful operation");
        }
        catch (Exception)
        {

            throw;
        }
    }

    /// <summary>
    /// Update an action by identifier
    /// </summary>
    /// <param name="id">action identifier</param>
    /// <param name="dto">action details</param>
    /// <returns></returns>
    public async Task Update(ActionsDto dto, int id)
    {
        try
        {
            if (dto == null)
                throw new ArgumentNullException(typeof(ActionsDto).Name);
            var action = _ActionsRepository.GetById(id);
            if (action == null)
                throw new EntityNotFoundException(string.Format("cannot find an entity {0} with the identifier {1} ", typeof(Actions), id));
            action.Name = dto.Name;
            action.Description = dto.Description;
            action.Points = dto.Points;
            action.ReducedCarb = dto.ReducedCarb;
            action.CategoryId = dto.CategoryId;
            await _ActionsRepository.UpdateAsync(action);
            throw new SuccessfulOperationException("Successful operation");
        }
        catch (Exception)
        {
            throw;
        }

    }
    #endregion
}
